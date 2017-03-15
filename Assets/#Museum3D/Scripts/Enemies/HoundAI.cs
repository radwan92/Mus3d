using UnityEngine;

namespace Mus3d
{
    public class HoundAI : EnemyAI
    {
        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        protected override void InitializeBaseInterval ()
        {
            m_baseUpdateInterval = new WaitForSeconds (0.1f);
        }

        /* ---------------------------------------------------------------------------------------------------------------------------------- */
        protected override void InitializeStates ()
        {
            m_statesByType.Add (AiState.Type.Idle,      new State_Idle_Hound (m_enemyObject));
            m_statesByType.Add (AiState.Type.Chasing,   new State_Chasing_Hound (m_enemyObject));
            m_statesByType.Add (AiState.Type.Attacking, new State_Attacking_Hound (m_enemyObject));
            m_statesByType.Add (AiState.Type.Hit,       new State_Hit (m_enemyObject));
            m_statesByType.Add (AiState.Type.Dying,     new State_Dying (m_enemyObject));
        }
    }
}