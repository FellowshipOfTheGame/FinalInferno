namespace FinalInferno {
    public enum TargetType {
        SingleAlly = 0, // Mira em um unico aliado vivo (incluindo a si próprio)
        MultiAlly = 1, // Mira em todos os aliados vivos
        SingleEnemy = 2, // Mira em um unico inimigo vivo
        MultiEnemy = 3, // Mira em todos os inimigos vivos
        Self = 4, // Mira em si mesmo
        DeadAlly = 5, // Mira em um unico aliado morto
        DeadAllies = 6, // Mira em todos os aliados mortos
        AllAllies = 7, // Mira em todos os aliados, vivos ou mortos
        DeadEnemy = 8, // Mira em um unico inimigo morto
        DeadEnemies = 9, // Mira em todos os inimigos mortos
        AllEnemies = 10, // Mira em todos os inimigos, vivos ou mortos
        Null = 11
    }
}