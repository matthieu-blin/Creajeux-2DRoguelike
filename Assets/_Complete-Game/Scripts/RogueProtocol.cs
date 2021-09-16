public enum RogueProtocol
{
    InitGame, //cross seed //fixera le procedural

    #region Actions
    //Move, //
    Attack, // va declencher l'anim d'attaque
    #endregion
    #region Data
    Position, //float x, y : quand la position change, on envoie la pos
    Food, //int,  quand food change 
    HP, //pour les buissons

    //qui va gerer les ias ?
    #endregion



}
