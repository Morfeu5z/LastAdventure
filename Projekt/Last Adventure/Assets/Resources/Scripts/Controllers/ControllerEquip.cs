using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerEquip : MonoBehaviour {

    private GameObject hero;
    private Config config;
    private GameObject weapon;
    private GameObject head;
    private GameObject PrefubHand;
    private GameObject PrefubGun;
    private GameObject PrefubGasMaskFace;
    private GameObject audioNext;
    private GameObject audioSelect;
    private GameObject mask;

    private bool eqMove = true;
    private int x = 0, y = 0;

    string s_pos = "00";
    int i_pos = 1;
    string[] childsTab;
 

// Use this for initialization
void Start ()
    {
        hero        =  GameObject.Find("Hero").gameObject;
        weapon      =  GameObject.Find("Hero/Weapon").gameObject;
        head        =  GameObject.Find("Hero/Head").gameObject;
        config      =  GameObject.Find("Config").GetComponent<Config>();

        PrefubGun   = (GameObject)Resources.Load("prefabs/Items/Gun", typeof(GameObject));
        PrefubHand  = (GameObject)Resources.Load("prefabs/Items/Hand", typeof(GameObject));
        PrefubGasMaskFace = (GameObject)Resources.Load("prefabs/Items/GasMaskFace", typeof(GameObject));
        audioNext   = (GameObject)Resources.Load("Audio/Effects/Prefab/Prefab_AudioNext", typeof(GameObject));
        audioSelect = (GameObject)Resources.Load("Audio/Effects/Prefab/Prefab_AudioSelect", typeof(GameObject));


        config.ActiveEquip();
        hero.GetComponent<ControllerPlayer>().equip = this.gameObject;

        childsTab = new string[transform.childCount];
        for (int i = 0; i < transform.childCount; i++) childsTab[i] = transform.GetChild(i).transform.name;

        gameObject.SetActive(false);
    }

    private void Update()
    {
        EquipController();
    }

    public void Active(bool active, string name)
    {
        switch (name)
        {
            case "Gun":
                if (active)
                {
                    Instantiate(PrefubGun, weapon.transform, false);
                    hero.GetComponent<Animator>().SetBool("HaveWeapon", true);
                    Active(false, "Hand");
                }
                else
                {
                    Destroy(GameObject.Find("Hero/Weapon/Gun(Clone)").gameObject);
                    hero.GetComponent<Animator>().SetBool("HaveWeapon", true);
                    Active(true, "Hand");
                }
                break;

            case "Hand":
                if (active) Instantiate(PrefubHand, weapon.transform, false);
                else
                {
                    if (GameObject.Find("Hero/Weapon").transform.childCount > 0)
                    {
                        Destroy(GameObject.Find("Hero/Weapon").transform.GetChild(0).gameObject);
                    }
                }
                break;

            case "GasMask":
                if (active) mask = Instantiate(PrefubGasMaskFace, head.transform, false);
                else
                {
                    Destroy(mask);
                }
                break;

            case "_Dead_":
                if (GameObject.Find("Hero/Weapon").transform.childCount > 0)
                {
                    Destroy(GameObject.Find("Hero/Weapon").transform.GetChild(0).gameObject);
                }
                break;
        }
    }

    public int FindMe(string pos = "")
    {
        for (int i = 0; i < childsTab.Length; i++) if (pos == childsTab[i]) return i;
        return -1;
    }

    public void Move(string go = "")
    {
        if (go == "up" || go == "down")
        {
            int add = 0;
            if (go == "up" && y > 0) add = -1;
            else if (go == "down" && y < 3) add = 1;
            if (add != 0)
            {
                y += add;
                s_pos = y.ToString() + x.ToString();
                i_pos = FindMe(s_pos);
                //Debug.Log("Eq spos: " + s_pos);
                transform.GetChild(0).gameObject.transform.position = transform.GetChild(i_pos).gameObject.transform.position;
                Destroy(Instantiate(audioNext, transform, false), 0.3f);
            }
        }
        else if (go == "left" || go == "right")
        {
            int add = 0;
            if (go == "left" && x > 0) add = -1;
            else if (go == "right" && x < 4) add = 1;
            if (add != 0)
            {
                x += add;
                s_pos = y.ToString() + x.ToString();
                i_pos = FindMe(s_pos);
                transform.GetChild(0).gameObject.transform.position = transform.GetChild(i_pos).gameObject.transform.position;
                Destroy(Instantiate(audioNext, transform, false), 0.3f);
            }
        }
        eqMove = false;
    }

    public void EquipController()
    {
        if (Input.GetAxisRaw("Vertical") == 0 && Input.GetAxisRaw("Horizontal") == 0) eqMove = true;
        if (eqMove)
        {
            if (Input.GetAxisRaw("Horizontal") > 0.4) Move("right");
            else if (Input.GetAxisRaw("Horizontal") < -0.4) Move("left");
            else if (Input.GetAxisRaw("Vertical") < -0.4) Move("down");
            else if (Input.GetAxisRaw("Vertical") > 0.4) Move("up");
            else if (Input.GetButtonDown("Accept"))
            {
                if (transform.GetChild(i_pos).transform.childCount > 0) {
                    config.UseItem(transform.GetChild(i_pos).transform.GetChild(0).gameObject, i_pos);
                }
                Destroy(Instantiate(audioSelect, transform, false), 0.5f);
            }

            if (transform.GetChild(i_pos).transform.childCount > 0)
            {
                ItemInfo item = transform.GetChild(i_pos).transform.GetChild(0).GetComponent<ItemInfo>();
                config.GetComponent<Config>().MessageBox(true, item.item.Description, item.item.ItemName, "Sprites/Items/Items/" + item.item.ObjectName);
            }
            else
            {
                config.MessageBox(false);
            }
        }
    }
}
