using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : Singleton<PlayerAbilities>
{

    public int tripodCooldownTime;
    public int nubsCooldownTime;

    [Header("Hover Legs")]
    bool hovering;
    public float hoverLegsDuration;
    public int hoverLegsCooldownTime;

    [Header("Basic Dash")]
    public Ease dashEase;
    bool dashing;
    float dashStartTime;
    public float dashDuration;
    public float dashPower;

    [Header("Basic Dash")]
    bool spawnedTrail;
    [SerializeField]
    GameObject slugLeg_trail;
    bool spawningSlugTrail;
    [SerializeField]
    float timeBetweenTrail;
    [SerializeField]
    int slugCooldownTime;

    bool isOnCoolDown;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        #region Basic Dash
        if (dashing)
        {
            //dashParticles.SetActive(true);
            print("Dash");
            float timeSinceDash = Time.time - dashStartTime;

            //turn off if u dont want trail render
            //turn on trail render
            _PC.GetComponent<TrailRenderer>().enabled = true;

            if (timeSinceDash >= dashDuration)
            {
                dashing = false;
            }
            else
            {
                float knockbackProgress = timeSinceDash / dashDuration;

                var dashDiresction = _PC.move * dashPower;
                dashDiresction = new Vector3(dashDiresction.x, 0, dashDiresction.z);

                //_PC.transform.DOMove(dashDiresction, 1).SetEase(dashEase);
                _PC.controller.Move(dashDiresction * Time.deltaTime); //(WORKS BUT VERY SNAPPY)

            }
        }
        else
        {
            //turn off if u dont want trail render
            _PC.GetComponent<TrailRenderer>().enabled = false;
            //dashParticles.SetActive(false);
        }

        #endregion

        #region Slug Trail
        if(spawningSlugTrail)
        {
            if (!spawnedTrail)
            {
                spawnedTrail = true;
                GameObject trail = Instantiate(slugLeg_trail, _PC.transform.position, Quaternion.identity);
                trail.GetComponent<SlugLegs>().enabled = false;
                ExecuteAfterFrames(10, () => trail.GetComponent<SlugLegs>().enabled = true);
                ExecuteAfterSeconds(timeBetweenTrail, () => spawnedTrail = false);
            }
        }
        

        #endregion
    }

    public void CallAbility(Item _item)
    {
        if(!isOnCoolDown)
        {
            switch (_item.ID)
            {
                case 1:
                    NubsAbility();

                    break;
                case 5:
                    TripodAbility();

                    break;
                
                case 6:
                    SlugAbility();

                    break;
            }
        }

    }

    public void NubsAbility()
    {
        _EI.LegAvatar.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Ability");

        isOnCoolDown = true;
        _AM.nubsAbility.Play();
        _PE.nubsPS.Play();
        Dash(3, 0.3f);


        ////OPTIONAL: Invunerable while dashing
        _PC.immortal = true;
        ExecuteAfterSeconds(0.3f, () => _PC.immortal = false);

        ActivateCooldownUI(nubsCooldownTime);
        ExecuteAfterSeconds(nubsCooldownTime, () => isOnCoolDown = false);

    }

    public void TripodAbility()
    {
        _EI.LegAvatar.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Ability");

        Dash(5, 0.3f);
        _AM.tripodAbility.Play();

        _PE.TripodVFX();

        //CoolDown
        isOnCoolDown = true;

        //turn on cooldown UI
        ActivateCooldownUI(tripodCooldownTime);


        ////OPTIONAL: Invunerable while dashing
        //_PC.immortal = true;
        //ExecuteAfterSeconds(0.3f, () => _PC.immortal = false);

        ExecuteAfterSeconds(tripodCooldownTime, () => isOnCoolDown = false);
    }

    public void SlugAbility()
    {
        isOnCoolDown = true;
        ActivateCooldownUI(slugCooldownTime);


        spawningSlugTrail = true;
        ExecuteAfterSeconds(slugCooldownTime, () => isOnCoolDown = false);

        ExecuteAfterSeconds(5, () => spawningSlugTrail = false);
  
    }

    void HoverAbility()
    {
        print("HOVERING");

        isOnCoolDown = true;
        //turn on cooldown UI
        ActivateCooldownUI(hoverLegsCooldownTime);

        hovering = true;
        _PC.canFloat = true;
        ExecuteAfterSeconds(hoverLegsDuration, () => ResetHover());
        ExecuteAfterSeconds(hoverLegsCooldownTime, () => isOnCoolDown = false);

    }

    void ResetHover()
    {
        hovering = false;
        _PC.canFloat = false;
    }

    public void Dash(float _dashPower, float _dashDuration)
    {
        dashPower = _dashPower;
        dashDuration = _dashDuration;
        dashStartTime = Time.time;
        dashing = true;
        print("dashing set true");

        //start cooldown here
    }

    void ActivateCooldownUI(int _cooldown)
    {
        _HUD.isCooldown3 = true;
        _HUD.SetCooldownSlo3(_cooldown);
    }
}