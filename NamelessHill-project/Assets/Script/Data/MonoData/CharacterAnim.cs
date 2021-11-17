using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.DataMono
{
    public class CharacterAnim : MonoBehaviour
    {
        private PawnAvatar _pawn;
        public Animator _animator;

        public void Init(PawnAvatar pawn)
        {
            this._pawn = pawn;

            if (this._pawn)
            {
                this._pawn._waitEvent += this.Wait;
                this._pawn._walkEvent += this.Walk;
                this._pawn._attackEvent += this.Attack;
                this._pawn._deathEvent += this.Death;
            }
        }
        private void Wait()
        {
            if (_animator == null) return;
            this._animator.SetBool("Attack", false);
            this._animator.SetBool("Walk", false);
        }
        private void Attack()
        {
            if (_animator == null) return;
            this._animator.SetBool("Walk", false);
            this._animator.SetBool("Attack", true);
        }

        private void Walk()
        {
            if (_animator == null) return;
            this._animator.SetBool("Attack", false);
            this._animator.SetBool("Walk", true);
        }

        private void Death()
        {
            if (_animator == null) return;
            this._animator.SetTrigger("Death");
        }

        private void OnDestroy()
        {
            if (this._pawn)
            {
                try
                {
                    //_pawn.pawn.actionAttack -= Attack;
                    this._pawn._waitEvent -= this.Wait;
                    this._pawn._walkEvent -= this.Walk;
                    this._pawn._attackEvent -= this.Attack;
                    this._pawn._deathEvent -= this.Death;
                }
                catch
                {
                }
            }
        }

        #region//¶¯»­ÊÂ¼þ
        public void DeathEvent()
        {
            this._pawn.ClearPawn();
        }
        #endregion
    }
}