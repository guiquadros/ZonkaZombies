using Assets.Scripts.Cenario.Room.Door;
using UnityEngine;

namespace Assets.Scripts.UI.Door
{
    /// <summary>
    /// Esta classe é responsável por prover acesso a UI que apresenta opções logo acima da porta.
    /// </summary>
    [DisallowMultipleComponent]
    public class DoorUIHandler : MonoBehaviour
    {
        /// <summary>
        /// Apresenta a UI logo acima da porta selecionada, com as devidas opções.
        /// </summary>
        /// <param name="doorType"></param>
        /// <param name="doorState"></param>
        /// <param name="doorPosition"></param>
        public void Show(DoorTypeEnum doorType, DoorStateEnum doorState, Vector3 doorPosition)
        {
            Vector3 uiPos = UnityEngine.Camera.main.WorldToScreenPoint(doorPosition + Vector3.up * 2f);
            uiPos.z = 0;

            //TODO Apresentar UI conforme especificações de parâmetros
        }
    }
}