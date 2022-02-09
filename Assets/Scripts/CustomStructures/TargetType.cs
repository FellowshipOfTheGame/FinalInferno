namespace FinalInferno {
    public enum TargetType {
        SingleAlly, // Mira em um unico aliado (incluindo a si próprio)
        MultiAlly, // Mira em todos os aliados vivos
        SingleEnemy, // Mira em um unico inimigo
        MultiEnemy, // Mira em todos os inimigos vivos
        Self, // Mira em si mesmo
        DeadAlly, // Mira em um unico aliado morto
        DeadAllies, // Mira em todos os aliados mortos
        AllAllies, // Mira em todos os aliados, vivos ou mortos
        DeadEnemy, // Mira em um unico inimigo morto
        DeadEnemies, // Mira em todos os inimigos mortos
        AllEnemies, // Mira em todos os inimigos, vivos ou mortos
        Null
    }
}