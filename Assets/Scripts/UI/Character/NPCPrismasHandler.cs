using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Character
{
    public class NPCPrismasHandler : MonoBehaviour
    {
        [SerializeField]
        private Image[] prismas;

        private void Awake()
        {
            if (prismas == null)
                throw new System.NullReferenceException("ERRO: A array \"prismas\" deve ser inicializado!");

            if (prismas.Length != 3)
                throw new System.NullReferenceException("ERRO: O array \"prismas\" deve conter exatamente 3 elementos!");
        }

        /// <summary>
        /// Apresenta a quantidade de prismas informadas. O valor mínimio dos prismas é de 1 e o máximo é de 3.
        /// Por padrão, ao chamar o método sem enviar parâmetros, apenas 1 prisma é apresentado.
        /// </summary>
        /// <param name="count"></param>
        public void ShowPrismas(int count = 1)
        {
            count--;
            for (int i = 0; i < prismas.Length; i++)
           {
                if (i > count)
                    prismas[i].enabled = false;
                else
                    prismas[i].enabled = true;
            }
        }

    }

}