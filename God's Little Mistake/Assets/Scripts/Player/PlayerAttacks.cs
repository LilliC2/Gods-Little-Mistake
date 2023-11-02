using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerAttacks : Singleton<PlayerAttacks>
{


    [Header("Projectile")]
    public Vector3 target;
    bool projectileShot;

    [Header("Sabertooth Projectile")]
    public bool returned = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CallAttack(Item _item)
    {
        switch (_item.ID)
        {
            case 0: //Pea Shooter

                PeaShooterAttack();

                break;
            
            case 2: //Sabertooth

                print("Saberooth");

                SabertoothAttack();

                break;
                
            case 3: //Sabertooth

                print("Lob");

                BasicLobProjectile(_IM.itemDataBase[3].projectileRange, _IM.itemDataBase[0].projectileSpeed, _IM.itemDataBase[0].projectilePF);

                break;



        }

    }

    public void PeaShooterAttack()
    {
        _PE.peaShooterPS.Play();
        BasicFireProjectile(_IM.itemDataBase[0].projectilePF, _IM.itemDataBase[0].projectileSpeed, _IM.itemDataBase[0].firerate, _IM.itemDataBase[0].projectileRange);

    }




    public void SabertoothAttack()
    {
        if(returned)
        {
            returned = false;
            BasicFireProjectile(_IM.itemDataBase[2].projectilePF, _IM.itemDataBase[2].projectileSpeed, _IM.itemDataBase[2].firerate, _IM.itemDataBase[2].projectileRange);
            ExecuteAfterSeconds(1, () => returned = true);
        }
        

    }

    public void BasicLobProjectile(float _range, float _projectileSpeed, GameObject _prefab)
    {

        GameObject bullet = Instantiate(_prefab, _PC.headFiringPoint.transform.position, _PC.headFiringPoint.transform.rotation);

        //Py =0
        //Px = _range
        //A = launch point
        //H = max vertical displacement
        //g = -17 m/s2



    }

    public void BasicFireProjectile(GameObject _prefab, float _projectileSpeed, float _firerate, float _range)
    {
        Vector3 screenPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 cursorRay = Camera.main.ScreenPointToRay(Input.mousePosition).direction;
        Vector3 flatAimTarget = screenPoint + cursorRay / Mathf.Abs(cursorRay.y) * Mathf.Abs(screenPoint.y - transform.position.y);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            _PC.headFiringPoint.transform.LookAt(hit.point);

            if (!projectileShot)
            {
                print("Fire");
                //particle system

                //Spawn bullet and apply force in the direction of the mouse
                //Quaternion.LookRotation(flatAimTarget,Vector3.forward);
                GameObject bullet = Instantiate(_prefab, _PC.headFiringPoint.transform.position, _PC.headFiringPoint.transform.rotation);
                bullet.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * _projectileSpeed);

                //bullet.GetComponent<ItemLook>().firingPoint = _PC.headFiringPoint;
                bullet.GetComponent<RangeDetector>().range = _range;
                bullet.GetComponent<RangeDetector>().positionShotFrom = _PC.transform.position;

                //knockbackActive = true;
                //knockbackStartTime = Time.time;
                Mathf.Clamp(bullet.transform.position.y, 0, 0);



                //Controls the firerate, player can shoot another bullet after a certain amount of time
                projectileShot = true;

                ExecuteAfterSeconds(_firerate, () => projectileShot = false);
            }
            print("FIRE PROJECTILE");

        }
    }

}