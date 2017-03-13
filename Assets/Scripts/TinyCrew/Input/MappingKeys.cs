namespace TinyCrew.Input
{
    internal sealed class MappingKeys
    {
        #region STICKS
        public string LeftStickHorizontal;
        public string LeftStickVertical;
        public string RightStickHorizontal;
        public string RightStickVertical;
        #endregion

        #region DIRECTIONAL PAD (D-PAD)
        public string DPadHorizontal;
        public string DPadVertical;
        #endregion

        #region AXIS TRIGGERS
        /// <summary>
        /// Right Trigger (RT)
        /// </summary>
        public string RightTrigger;
        /// <summary>
        /// Left Trigger (LT)
        /// </summary>
        public string LeftTrigger;
        #endregion

        #region BUTTONS
        public string LeftStickButton;
        public string RightStickButton;

        public string A;
        public string B;
        public string X;
        public string Y;

        public string Back;
        public string Start;
        #endregion

        #region BUMPERS
        /// <summary>
        /// Left Bumper (LB)
        /// </summary>
        public string LeftBumper;
        /// <summary>
        /// Right Bumper (RB)
        /// </summary>
        public string RightBumper;
        #endregion
    }
}
