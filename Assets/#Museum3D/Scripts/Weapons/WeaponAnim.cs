using System;
using System.Collections;
using UnityEngine;

namespace Mus3d
{
    public class WeaponAnim
    {
        public event Action E_Shot;
        public event Action E_ShotFinished;

        public bool InAction { get; private set; }

        Weapon          m_weapon;
        SpriteRenderer  m_renderer;

        WaitForSeconds m_weaponRaiseDelay  = new WaitForSeconds (0.1f);
        WaitForSeconds m_weaponLowerDelay  = new WaitForSeconds (0.07f);
        WaitForSeconds m_weaponRepeatDelay = new WaitForSeconds (0.15f);
        WaitForSeconds m_weaponShotDelay;
        WaitForSeconds m_weaponShotDuration;

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public WeaponAnim (SpriteRenderer weaponSpriteRenderer)
        {
            m_renderer = weaponSpriteRenderer;
            Coroutiner.Start (AnimationCoroutine ());
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public void ChangeWeapon (Weapon weapon)
        {
            if (InAction)
                return;

            m_weapon              = weapon;
            m_renderer.sprite     = weapon.IdleSprite;
            m_weaponShotDelay     = new WaitForSeconds (weapon.ShootDelay);
            m_weaponShotDuration  = new WaitForSeconds (weapon.ShotDuration);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public void Start ()
        {
            if (InAction)
                return;
            InAction = true;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        public void Stop ()
        {
            if (!InAction)
                return;
            InAction = false;
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        IEnumerator AnimationCoroutine ()
        {
            while (true)
            {
                while (InAction)
                {
                    m_renderer.sprite = m_weapon.AttackSprites[0];

                    yield return m_weaponRaiseDelay;

                    do
                    {
                        m_renderer.sprite = m_weapon.AttackSprites[1];

                        E_Shot ();

                        yield return m_weaponShotDuration;

                        m_renderer.sprite = m_weapon.AttackSprites[2];

                        yield return m_weaponShotDelay;

                        if (!m_weapon.IsRepeatable)
                            break;
                    }
                    while (InAction);

                    m_renderer.sprite = m_weapon.AttackSprites[3];

                    yield return m_weaponLowerDelay;

                    m_renderer.sprite = m_weapon.IdleSprite;

                    yield return m_weaponRepeatDelay;

                    E_ShotFinished ();
                }

                yield return null;
            }

        }
    }
}