using UnityEngine;

namespace Assets.Scripts.Character.Attribute
{
    public class Attribute : MonoBehaviour
    {
        public delegate void AttributeValueChanged(Attribute attribute, float oldValue);
        public AttributeValueChanged OnAttributeValueChanged;

        [SerializeField]
        private AttributeType type;
        public AttributeType Type { get { return type; } }

        [SerializeField]
        private float min, max, current;
        public float Min { get { return this.min; } }
        public float Max { get { return this.max; } }
        public float Current { get { return this.current; } set { this.current = value; } }

        [Header("Hack")]
        [SerializeField]
        private Attribute maldicaoAttribute;
        [SerializeField]
        private bool add;
        [SerializeField]
        private bool lose;

        public void Initialize(AttributeType type, float min, float current, float max)
        {
            this.type = type;
            this.min = min;
            this.current = current;
            this.max = max;
            UpdateGameObjectName();
        }

        private void Start()
        {
            UpdateGameObjectName();
        }

        private void Update()
        {
            if (add)
            {
                add = false;
                Add(.05f);
            } else if (lose)
            {
                lose = false;
                Lose(.05f);
            }
        }

        /// <summary>
        /// Incrementa o valor deste atributo com o valor declarado.
        /// </summary>
        /// <param name="value"></param>
        public void Add(float value)
        {
            float oldValue = current;
            current += value;
            ClampCurrentValue();
            DispatchOnValueChanged(oldValue);
        }

        /// <summary>
        /// Decrementa o valor deste atributo com o valor declarado.
        /// </summary>
        /// <param name="value"></param>
        public void Lose(float value)
        {
            float oldValue = current;
            current -= value;
            ClampCurrentValue();
            DispatchOnValueChanged(oldValue);

            if (maldicaoAttribute != null)
                maldicaoAttribute.Lose(value);
        }

        private void ClampCurrentValue()
        {
            current = Mathf.Clamp(current, min, max);
        }

        /// <summary>
        /// Torna o nome deste GameObject o valor recuperado pelo método "ToString" desta classe.
        /// </summary>
        private void UpdateGameObjectName()
        {
            gameObject.name = this.ToString();
        }

        public override string ToString()
        {
            return string.Format("{0} [min:{1},current:{2},max:{3}]", type, min, current, max);
        }

        private void DispatchOnValueChanged(float oldValue)
        {
            UpdateGameObjectName();

            if (OnAttributeValueChanged != null)
                OnAttributeValueChanged(this, oldValue);
        }
    }
}