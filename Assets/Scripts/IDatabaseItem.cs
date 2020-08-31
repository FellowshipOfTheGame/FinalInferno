using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FinalInferno{
    public interface IDatabaseItem
    {
        // Função chamada ao buildar, le os arquivos de tabelas e guarda como referencias serializadas
        void LoadTables();
        // Função chamada no inicio do jogo, pode ser que seja desnecessária
        void Preload();
    }
}