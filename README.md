# MiniSO

MiniSO é uma simulação de um sistema operacional simplificado, implementando conceitos de escalonamento, gerenciamento de memória, processos e threads.

> **Feito em:** .NET 8  
> **Integrantes:** Luiz Chiarinotti (113079) e Gustavo Tesche (113265)

## Visão Geral

O projeto é composto por seis principais classes:

- `Sistema`: Gerencia o ciclo de vida do sistema, inicialização, controle de processos, memória e escalonador.
- `Escalonador`: Responsável por decidir qual processo será executado, implementando políticas como Round-Robin (RR), Prioridade e FCFS (First-Come, First-Served).
- `Memoria`: Gerencia a alocação e liberação de memória para processos.
- `Segmento`: Representa um segmento com registradores base-limite.
- `Processador`: Simula o processador, controlando o estado de execução dos processos.
- `Processo`: Representa um processo, contendo informações como PID, prioridade, estado, tamanho de memória e suas threads.
- `Thread`: Representa uma thread pertencente a um processo, com seu próprio estado, prioridade e contador de instruções.


## Classes e Funções

### Sistema

- **Propriedades:**
	- `memoria`: Instância de `Memoria`.
	- `processador`: Instância de `Processador`.
	- `escalonador`: Instância de `Escalonador`.
	- `processos`: Lista de processos ativos.
	- `cts`: Token de cancelamento para controle de execução.
	- `ProcessosLock`: Objeto para sincronização de acesso à lista de processos.
	- `pauseEvent`: Controle de pausa do sistema.

- **Eventos:**
	- `ProcessoFinalizado`: Disparado quando um processo finaliza.
	- `ProcessoDesbloqueado`: Disparado quando um processo é desbloqueado por memória disponível.

- **Principais métodos:**
	- `IniciarSistema(int memoriaTotal, int autoCriarIntervalMs = 0, string politica = "RR")`: Inicializa o sistema, memória, escalonador e inicia o loop principal.
	- Gerenciamento de processos, pausa, retomada e finalização.

### Escalonador

- **Propriedades:**
	- `politica`: Política de escalonamento ("RR", "PRIORIDADE", "FCFS").
	- `quantum`: Quantum para RR e PRIORIDADE. (PRIORIDADE -> (Quantum + Prioridade))
	- `filaRR`: Fila de processos para escalonamentos que utilizam de quantum.

- **Eventos:**
	- `ProcessoTrocado`: Notifica troca de processo.

- **Principais métodos:**
	- `Escalonador(string politica, int quantum)`: Construtor.
	- `Escalonar(List<Processo> processos, int delayMs)`: Executa o escalonamento conforme política definida.
	- `Envelhecer(List<Processo> processos)`: Envelhece os processos incremento o AgeTicks dele conforme cada ciclo de escalonamento que se passa e o processo ainda não foi executado, alterando a PrioridadeDinamica para Alta, ao ser executado, AgeTicks do processo é zerado. (APENAS NO PRIORIDADE).
	- `RegistrarTrocaDeContexto`: Simula troca de contexto incrementando TrocasDeContexto em 1 a cada troca registrada e aguarda o tempo de custo configurável na interface.
	- `RegistrarTrocaDeContextoThread`: Mesmo comportamento do RegistrarTrocaDeContexto porém entre as threads

	
### Memoria

- **Propriedades:**
	- `total`: Memória total disponível.
	- `segmentos`: Lista dos segmentos.

- **Principais métodos:**
	- `Memoria(int total)`: Construtor.
	- `alocar(int tamanho)`: Tenta alocar um segmento de memória do tamanho especificado seguindo estratégia: Antes do primeiro segmento existente, Entre segmentos já alocados (buracos) e após o último segmento.
	- `liberar(int qtd)`: Libera o segmento que começa no endereço baseAddr.
	
### Segmento

- **Propriedades:**
	- `Base`: Endereço base do segmento.
	- `Limite`: Endereço limite do segmento.

- **Principais métodos:**
	- `Segmento(int baseAddr, int limit)`: Construtor.


### Processador

- **Principais métodos:**
	- `Processador(float freq)`: Construtor.
	- `ExecutarProcesso(Processo p)`: Inicia execução de um processo.
	- `LiberarProcesso(Processo p)`: Libera o processo em caso de interrupção.

### Processo

- **Propriedades:**
	- `pId`: Identificador do processo.
	- `estado`: Estado do processo (`Pronto`, `Executando`, `Bloqueado`, `Finalizado`).
	- `prioridade`: Prioridade do processo.
	- `tamanhoMemoria`: Memória ocupada.
	- `threads`: Lista de threads do processo.
	- `QuantumAtual`: Quantum atual para RR.
	- `PrioridadeDinamica`: Prioridade utilizada para o envelhecimento.
	- `AgeTicks`: Ticks de idade do processo para envelhecimento.
	- `EscalonadorInstance`: Instância do escalonador atual.
	- `Segmento Base`: Registrador de Base do processo.
	- `Segmento Limite`: Registrador de Limite do processo.
	- `TabelaSegmentos`: Tabela dos segmentos do processo.

- **Principais métodos:**
	- `Processo(int pid, Prioridade prioridade, int tamanho)`: Construtor.
	- `ExecutarRR(int quantum, Action onUnitExecuted = null, int delayPorUnidadeMs = 200)`: Executa o processo segundo RR, alternando entre threads.
    - `ExecutarFCFS(int delayPorUnidadeMs = 200, Action onUnitExecuted = null)`: Executa o processo segundo FCFS, onde, os processos e threads já são inseridos em ordem ao serem instanciados.

### Thread

- **Propriedades:**
	- `tId`: Identificador da thread.
	- `pIdPai`: Identificador do processo pai.
	- `tamanho`: Memória ocupada pela thread.
	- `estado`: Estado da thread.
	- `prioridade`: Prioridade da thread.
	- `pc`: Contador de instruções executadas.
	- `countPc`: Total de instruções a executar.

- **Principais métodos:**
	- `Thread(int tid, int pidPai, int tamanho, Prioridade prioridade, int countPc)`: Construtor.
	- `ExecutarUnidade()`: Executa uma unidade de instrução, retorna se finalizou (true/false).

## Relacionamentos

- O `Sistema` inicializa e conecta todos os componentes.
- O `Escalonador` decide qual `Processo` será executado pelo `Processador`.
- O `Processador` marca os estados dos processos, que por sua vez gerencia suas `Threads`.
- A `Memoria` controla a alocação e liberação para processos e threads.
- Eventos permitem comunicação entre componentes e atualização da interface.

## Funcionalidades implementadas

- ✅ Processos e Threads + (Simulador de Trocas de Contexto com Sobrecarga configurável + Contador)
- ✅ Escalonamento por Round-Robin (RR).
- ✅ Escalonamento por Prioridade (com quantum ajustável).
- ✅ Escalonamento por FCFS (First-Come, First-Served).
- ✅ Segmentação de Memória
- ✅ Log de eventos dos processos e memória

## Bônus
- ✅ Escalonamento por Prioridade com Algoritmo de Envelhecimento, evitando inanição.

## Bugs observados
- Foi observado que quando um processo com uma thread única executa, cada incremento do PC também incrementa Troca de Contexto, a tentativa de corrigir
este bug foi:

if (EscalonadorInstance.threadAtual?.tId != t.tId)
{
    await EscalonadorInstance.RegistrarTrocaDeContextoThread(
        EscalonadorInstance.threadAtual,
        t
    );
}

Garantindo que para que fosse incrementado a troca de contexto, a thread que estava no escalonador fosse diferente da thread sendo executada, porém o código
não surtiu efeitos.

<!-- 
# MiniSO

MiniSO é uma simulação de um sistema operacional simplificado, implementando conceitos de escalonamento, gerenciamento de memória, processos e threads.

> **Feito em:** .NET 8  
> **Integrantes:** Luiz Chiarinotti (113079) e Gustavo Tesche (113265)

## Visão Geral

O projeto é composto por seis principais classes:

- `Sistema`: Gerencia o ciclo de vida do sistema, inicialização, controle de processos, memória e escalonador.
- `Escalonador`: Responsável por decidir qual processo será executado, implementando políticas como Round-Robin (RR), Prioridade e FCFS (First-Come, First-Served).
- `Memoria`: Gerencia a alocação e liberação de memória para processos.
- `Processador (em desenvolvimento)`: Simula o processador, controlando o estado de execução dos processos. Atualmente não possui funcionalidade completa.
- `Processo`: Representa um processo, contendo informações como PID, prioridade, estado, tamanho de memória e suas threads.
- `Thread`: Representa uma thread pertencente a um processo, com seu próprio estado, prioridade e contador de instruções.

## Funcionalidades implementadas

- Escalonamento por Round-Robin (RR).
- Escalonamento por Prioridade (com quantum ajustável).
- Escalonamento por FCFS (First-Come, First-Served).
- Segmentação de Memória
- Simulação de execução de threads dentro dos processos.
- Eventos para comunicação interna (ex.: `ProcessoFinalizado`, `ProcessoDesbloqueado`, `ProcessoTrocado`).

## Arquitetura e responsabilidades (resumo)

- O `Sistema` inicializa e conecta todos os componentes, mantendo a lista de processos e os mecanismos de sincronização.
- O `Escalonador` decide qual `Processo` será executado pelo `Processador`, de acordo com a política ativa.
- O `Processador` executa o processo (simulação), que por sua vez gerencia suas `Threads`.
- A `Memoria` controla a alocação e liberação para processos e threads.
- Eventos permitem comunicação entre componentes e atualização da interface/controle.

## Estrutura das classes (detalhes)

### Sistema

- **Propriedades:** `memoria`, `processador`, `escalonador`, `processos`, `cts`, `ProcessosLock`, `pauseEvent`.
- **Eventos:** `ProcessoFinalizado`, `ProcessoDesbloqueado`.
- **Principais métodos:** `IniciarSistema(int memoriaTotal, int autoCriarIntervalMs = 0, string politica = "RR")`, gerenciamento de processos, pausa/retomada e finalização.

### Escalonador

- **Propriedades:** `politica` ("RR", "PRIORIDADE", "FCFS"), `quantum`, `filaRR`.
- **Eventos:** `ProcessoTrocado`.
- **Principais métodos:** Construtor `Escalonador(string politica, int quantum)` e `Escalonar(List<Processo> processos, int delayMs)`.

> Observação: a política `PRIORIDADE` atualmente aplica quantum baseado em `(quantum + prioridade)`.

### Memoria

- **Propriedades:** `total`, `livre`.
- **Principais métodos:** `Memoria(int total)`, `alocar(int qtd)`, `liberar(int qtd)`.

### Processador (em desenvolvimento)

- **Propriedades:** `freq`, `estado` (`Aguardando`, `Executando`).
- **Principais métodos:** `Processador(float freq)`, `ExecutarProcesso(Processo p)`, `LiberarProcesso(Processo p)`.

### Processo

- **Propriedades:** `pId`, `estado` (`Pronto`, `Executando`, `Bloqueado`, `Finalizado`), `prioridade`, `tamanhoMemoria`, `threads`, `QuantumAtual`.
- **Principais métodos:** Construtor `Processo(int pid, Prioridade prioridade, int tamanho)`,  
  `ExecutarRR(int quantum, Action onUnitExecuted = null, int delayPorUnidadeMs = 200)`,  
  `ExecutarFCFS(int delayPorUnidadeMs = 200, Action onUnitExecuted = null)`.

### Thread

- **Propriedades:** `tId`, `pIdPai`, `tamanho`, `estado`, `prioridade`, `pc`, `countPc`.
- **Principais métodos:** Construtor `Thread(int tid, int pidPai, int tamanho, Prioridade prioridade, int countPc)`, `ExecutarUnidade()`.

## Relacionamentos

- O `Sistema` inicializa e conecta todos os componentes.
- O `Escalonador` decide qual `Processo` será executado pelo `Processador`.
- O `Processador` executa o processo, que por sua vez gerencia suas `Threads`.
- A `Memoria` controla a alocação e liberação para processos e threads.
- Eventos permitem comunicação entre componentes e atualização da interface.

## Problemas conhecidos

- **Starvation em escalonamento por prioridade:** Atualmente, no modo de escalonamento por prioridade, processos de baixa prioridade podem sofrer *starvation* (não chegam a ser executados), pois processos de prioridade alta ocupam continuamente a CPU.

## Como executar (resumo rápido)

1. Requisitos: .NET 8 SDK instalado.  
2. Abrir solução no Visual Studio / `dotnet` CLI.  
3. `dotnet restore` e `dotnet build`.  
4. Executar o projeto principal (`dotnet run` ou via IDE).

## Falta implementar e correções (segundo a especificação)

⚪	1. Remover a classe Processador (estudar como ela se encaixa no SO)

⚪	2. Gerenciamento de memória - mínimo: paginação simples com páginas de tamanho fixo e política de alocação first fit ou best fit no mapa de molduras.

⚪	3. Entrada/Saída - modelo de dispositivo genérico de bloco e dispositivo de caractere. Fila de pedidos por  dispositivo, tempos de serviço simulados e geração de interrupções. Deve existir uma 
	API de E S bloqueante e não bloqueante com espera ativa proibida.

⚪	4. Sistema de Arquivos - Estrutura hierárquica com diretório raiz, diretórios e arquivos. Tabela de nós i simples ou FCB com metadados nome, tamanho, timestamps lógicos, permissões simplificadas, 
	alocação contígua ou lista encadeada de blocos.

⚪	5. Métricas - O simulador deve produzir estatísticas ao final. Tempo de retorno por processo, tempo de espera em pronto, tempo de resposta primeira CPU, utilização de CPU e de cada 
	dispositivo, taxa de falta de página, throughput, número de trocas de contexto e sobrecarga total de escalonamento. Gerar log textual com clock e eventos relevantes.
	⚪ tempo de retorno por processo
	⚪ tempo de espera em pronto
	⚪ tempo de resposta CPU
	⚪ utilização de CPU
	⚪ taxa de falta de página
	⚪ throughput
	⚪ numero de trocas de contexto
	⚪ log com clock e eventos

⚪	6. Entregáveis 
	⚪	diretório no github com readme.md atualizado e detalhado 
	⚪	diagrama de classes do sistema no readme.md -->

