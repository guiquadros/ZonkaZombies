using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Character.Attribute
{
    public class AttributePool : MonoBehaviour
    {
        private Dictionary<AttributeType, Attribute> attributes;

        private void Awake()
        {
            //Recupera todos os objetos do tipo Attribute, nos filhos deste objeto
            Attribute[] childrenAttributes = GetComponentsInChildren<Attribute>();

            attributes = new Dictionary<AttributeType, Attribute>();

            for (int i = 0; i < childrenAttributes.Length; i++)
            {
                if (attributes.ContainsKey(childrenAttributes[i].Type))
                {
                    Debug.Log(string.Format("O atributo \"{0}\" está duplicado!", childrenAttributes[i].Type));
                    continue;
                }

                attributes.Add(childrenAttributes[i].Type, childrenAttributes[i]);
            }
        }

        /// <summary>
        /// Retorna o attributo, caso este personagem o contenha.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Attribute GetAttribute(AttributeType type)
        {
            return attributes[type];
        }

    }

}
