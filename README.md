<!--# MiniSO

MiniSO é uma simulação de um sistema operacional simplificado, implementando conceitos de escalonamento, gerenciamento de memória, processos e threads.

## Arquitetura Geral

O projeto é composto por seis principais classes:

- `Sistema`: Gerencia o ciclo de vida do sistema, inicialização, controle de processos, memória e escalonador.
- `Escalonador`: Responsável por decidir qual processo será executado, implementando políticas como Round-Robin (RR) e Prioridade.
- `Memoria`: Gerencia a alocação e liberação de memória para processos.
- `Processador (em desenvolvimento)`: Simula o processador, controlando o estado de execução dos processos. Atualmente não possui funcionalidade.
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

### Memoria

- **Propriedades:**
	- `total`: Memória total disponível.
	- `livre`: Memória livre.

- **Principais métodos:**
	- `Memoria(int total)`: Construtor.
	- `alocar(int qtd)`: Tenta alocar memória, retorna sucesso ou falha.
	- `liberar(int qtd)`: Libera memória, garantindo não exceder o total.

### Processador (em desenvolvimento)

- **Propriedades:**
	- `freq`: Frequência do processador.
	- `estado`: Estado atual (`Aguardando`, `Executando`).

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
- O `Processador` executa o processo, que por sua vez gerencia suas `Threads`.
- A `Memoria` controla a alocação e liberação para processos e threads.
- Eventos permitem comunicação entre componentes e atualização da interface.-->

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
- Gerenciamento simples de memória (alocação e liberação).
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

### Causa provável

- A implementação atual favorece processos de alta prioridade sem mecanismos compensatórios (p.ex. não há envelhecimento/aging ou ajuste dinâmico de prioridades), o que permite que processos de alta prioridade sejam sempre escalonados antes dos de baixa prioridade.

### Impacto

- Processos de baixa prioridade podem nunca executar em cargas onde há processos de alta prioridade constantemente prontos, comprometendo justiça e avaliação correta do sistema.

### Mitigações sugeridas (próximos passos)

1. **Aging (Envelhecimento):** Incrementar a prioridade de processos que ficam muito tempo na fila de pronto.  
2. **Time slice (limite de fatia de CPU):** Garantir que nenhum processo (mesmo de alta prioridade) possa monopolizar a CPU por mais que X unidades consecutivas.  
3. **Priority decay:** Reduzir a prioridade dos processos que consumiram muita CPU recentemente.  
4. **Multilevel Feedback Queue (MLFQ):** Implementar filas múltiplas com regras de promoção/rebaixamento.  
5. **Combinação RR+Prioridade:** Aplicar quantum fixo por faixa de prioridade e usar Round-Robin dentro de cada faixa.

> **Recomendação imediata:** implementar **Aging** + limite de fatia (time slice). Isso é rápido, tem baixo impacto no design atual e resolve o problema de starvation na maior parte dos cenários.

## Como executar (resumo rápido)

1. Requisitos: .NET 8 SDK instalado.  
2. Abrir solução no Visual Studio / `dotnet` CLI.  
3. `dotnet restore` e `dotnet build`.  
4. Executar o projeto principal (`dotnet run` ou via IDE).

## Testes e validação

- Há simulações internas para validar as políticas (RR, PRIORIDADE, FCFS). Recomenda-se criar cenários automatizados adicionais para reproduzir starvation e validar as mitigações propostas.

## Observações finais

Este README foi atualizado para incluir informações de versão (.NET 8), integrantes do projeto, suporte explícito a FCFS e a documentação do problema conhecido no escalonamento por prioridade, com propostas de mitigação.


