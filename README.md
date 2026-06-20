# LaunchBox — RomM Integration Plugin

Seamless integration between RomM Server and LaunchBox / BigBox — Integração entre RomM Server e LaunchBox / BigBox.

This README focuses on how to use the plugin from a LaunchBox user perspective (installing games, syncing and common tasks). No build or development instructions are included.

English and Portuguese (PT-BR) sections are provided so you can follow in the preferred language.

---

English (Quick User Guide)
==========================

Overview
--------
The LaunchBox RomM Plugin connects LaunchBox/BigBox to a RomM server so you can:
- Sync platforms and games from the RomM server
- Auto-fill LaunchBox metadata from the RomM server (release date, genre, company, rating, ESRB, synopsis, etc.) with priority: LaunchBoxMetadata > ScreenScraper > IGDB
- Download cover art (Box - Front) automatically from the RomM server
- Install and uninstall games directly from LaunchBox
- Push LaunchBox metadata and cover art back to the RomM server

Quick start
-----------
1. Download the plugin release and extract the plugin folder into your LaunchBox Plugins directory. Typical destination:

   LaunchBox/Plugins/RomM LaunchBox Integration

2. Configure connection settings (see Settings below). You can edit settings.json inside the plugin folder or configure via the LaunchBox plugin settings UI.

3. Open LaunchBox and use the main menu command:

   RomM: Sync roms list from server

4. After sync you will see platforms and games created or updated in LaunchBox. Each synced game gets two extra actions: Install (RomM) and Uninstall (RomM).

Settings (fields)
-----------------
- RommBaseUrl: URL of your RomM server (e.g. http://192.168.1.100:9000)
- Username / Password: RomM credentials (if your server requires auth)
- RomsPath: Local path where games should be installed (e.g. D:\\LaunchBox\\Games)
- KeepLocalData: When true, sync only fills empty metadata fields and downloads cover only if none exists. When false, sync overwrites all fields and replaces existing cover.

How sync works
--------------
- Platforms and games are requested from RomM and mirrored into LaunchBox. If a LaunchBox platform does not exist, the plugin creates it.
- The plugin stores RomM metadata into custom fields so LaunchBox items retain origin information.
- Metadata is applied with priority: LaunchBoxMetadata > ScreenScraper > IGDB > RomM Metadata.
- Cover art (Box - Front) is downloaded from the server only if the game has no existing cover in LaunchBox.
- When KeepLocalData is true, only empty/null fields are filled; existing data is preserved. When false, all fields are overwritten.
- Games that no longer exist on the server are removed from LaunchBox and their orphan images are cleaned up.
- After sync completes, LaunchBox reloads its library automatically via ForceReload so new metadata and images appear immediately.

Metadata fields synchronized (when available from the server):
- Release date, max players, play mode, community rating, community rating votes
- Video URL (YouTube), Wikipedia URL, ESRB rating, synopsis / notes
- LaunchBox ID mapping (LaunchBoxDbId) for cross-reference

Install / Uninstall flow
------------------------
- Install (RomM) — downloads the game package from RomM, extracts ZIPs when needed, applies folder layout fixes, parses an optional _launchbox.json and sets ApplicationPath automatically.
- Uninstall (RomM) — removes the installed files, clears ApplicationPath in LaunchBox and marks the game as uninstalled.

Actions are implemented as LaunchBox applications that call the bundled CLI:

  RommPlugin.CLI.exe install <gameId>
  RommPlugin.CLI.exe uninstall <gameId>

_launchbox.json (advanced per-game config)
-----------------------------------------
If a package contains a _launchbox.json file the plugin uses it to configure the game's files and applications. Typical keys:

- DefaultFileName — executable used as main application (relative to the game folder)
- HasDLC — whether the game bundle contains DLC that the plugin must treat specially
- AdditionalApplications — list of extra LaunchBox applications (Name, Path, optional CommandLine)
- PreLoaders / PosLoaders — programs or scripts to run before/after the main application

Placeholders like %romsFolder% are supported inside command lines and paths to point to the installed game folder.

Background events
-----------------
The plugin queues install/uninstall requests into a romm.sync file. The background worker processes the queue, updates installed status and removes finished events. The sync file is removed when there are no pending events.

CLI (silent)
------------
The included CLI performs install/uninstall tasks without showing a console window so LaunchBox actions run smoothly.

Troubleshooting / Common Issues
-------------------------------
- Server unreachable: check RommBaseUrl and that the RomM server is running and accessible on the network.
- Authentication failed: verify Username/Password and try via the RomM web UI.
- Permission errors writing files: ensure RomsPath is writable by your user and LaunchBox.
- Missing extracted EXE: check _launchbox.json DefaultFileName or inspect the extracted folder to find the correct executable.

Tips
----
- Keep RomsPath on a drive with enough free space.
- Use _launchbox.json for complex multi-file games so the plugin configures applications properly.
- Back up LaunchBox database before large syncs if you rely on custom edits.

FAQ
---
Q: How do I force a full resync?
A: Use the RomM: Sync roms list from server option. If needed, remove problematic platforms/games from LaunchBox and run sync again.

Q: Can I revert metadata the plugin wrote?
A: Metadata is stored in LaunchBox fields; you can edit or remove it from individual game entries via LaunchBox.

Português (Guia Rápido ao Usuário)
==================================

Visão geral
-----------
O plugin conecta o LaunchBox/BigBox ao servidor RomM permitindo:
- Sincronizar plataformas e jogos do servidor RomM
- Preencher automaticamente os metadados do LaunchBox com dados do servidor (data, gênero, empresa, rating, ESRB, sinopse, etc.) com prioridade: LaunchBoxMetadata > ScreenScraper > IGDB
- Baixar capa (Box - Front) automaticamente do servidor RomM
- Instalar e desinstalar jogos diretamente pelo LaunchBox
- Enviar metadados e capa do LaunchBox de volta ao servidor RomM

Início rápido
-------------
1. Baixe o plugin e extraia a pasta do plugin dentro da pasta de Plugins do LaunchBox:

   LaunchBox/Plugins/RomM LaunchBox Integration

2. Configure as opções de conexão (veja Configurações). Você pode editar settings.json na pasta do plugin ou usar a UI de plugins do LaunchBox.

3. Abra o LaunchBox e use o comando no menu:

   RomM: Sync roms list from server

4. Após a sincronização, plataformas e jogos aparecerão no LaunchBox. Cada jogo sincronizado ganha as ações Install (RomM) e Uninstall (RomM).

Configurações (campos)
---------------------
- RommBaseUrl: endereço do servidor RomM (ex.: http://192.168.1.100:9000)
- Username / Password: credenciais do RomM (se necessário)
- RomsPath: pasta local onde os jogos serão instalados (ex.: D:\\Jogos)
- KeepLocalData: quando true, a sync só preenche campos vazios e baixa capa se não existir. Quando false, sobrescreve tudo.

Como a sincronização funciona
---------------------------
- Plataformas e jogos são obtidos do RomM e espelhados no LaunchBox. Plataformas novas são criadas automaticamente quando necessário.
- Metadados do RomM são gravados em campos personalizados no LaunchBox para manter rastreabilidade.
- Metadados seguem a prioridade: LaunchBoxMetadata > ScreenScraper > IGDB > RomM Metadata.
- Capa (Box - Front) é baixada do servidor apenas se o jogo não tiver capa no LaunchBox.
- Com KeepLocalData=true, só campos vazios são preenchidos. Com false, todos os campos são sobrescritos.
- Jogos que não existem mais no servidor são removidos do LaunchBox e suas imagens órfãs são deletadas.
- Após a sync, o LaunchBox recarrega a biblioteca automaticamente via ForceReload para que os metadados e capas apareçam na hora.

Campos de metadados sincronizados (quando disponíveis no servidor):
- Data de lançamento, número máximo de jogadores, modo de jogo, vídeo (YouTube), rating comunitário
- Wikipedia, ESRB, sinopse / notas
- LaunchBox ID (LaunchBoxDbId) para referência cruzada

Fluxo de Instalar / Desinstalar
------------------------------
- Install (RomM) — baixa o pacote do RomM, extrai ZIPs, ajusta hierarquia, aplica configurações do _launchbox.json (se houver) e configura ApplicationPath.
- Uninstall (RomM) — remove arquivos instalados, limpa ApplicationPath no LaunchBox e marca o jogo como não instalado.

Ações são adicionadas como aplicações do LaunchBox que executam o CLI incluso:

  RommPlugin.CLI.exe install <gameId>
  RommPlugin.CLI.exe uninstall <gameId>

_launchbox.json (config avançada por jogo)
----------------------------------------
Se o pacote contém _launchbox.json o plugin usa essas configurações para montar aplicações e paths do jogo. Chaves comuns:

- DefaultFileName — executável principal (relativo à pasta do jogo)
- HasDLC — indica se o pacote contém DLCs
- AdditionalApplications — aplicativos adicionais (Name, Path, CommandLine)
- PreLoaders / PosLoaders — processos a rodar antes/depois do jogo

Suporte a placeholders como %romsFolder% para indicar a pasta instalada do jogo.

Processamento em segundo plano
------------------------------
Eventos de instalar/desinstalar são enfileirados no arquivo romm.sync. O worker processa a fila, atualiza status e remove eventos concluídos. O arquivo é deletado se não houver eventos pendentes.

CLI (silencioso)
----------------
O CLI incluido executa operações sem abrir janela de console, permitindo que as ações do LaunchBox rodem sem interrupções.

Resolução de problemas comuns
----------------------------
- Servidor inacessível: verifique RommBaseUrl e se o servidor RomM está online e acessível.
- Autenticação: verifique usuário e senha no settings.json ou na UI do plugin.
- Erros de permissão: confirme se a pasta RomsPath tem permissões de escrita para o usuário do LaunchBox.
- Executável não encontrado: verifique DefaultFileName no _launchbox.json ou abra a pasta extraída para localizar o .exe correto.

Dicas
-----
- Mantenha RomsPath em um disco com espaço suficiente.
- Use _launchbox.json para jogos com múltiplos arquivos ou aplicações secundárias.
- Faça backup do banco de dados do LaunchBox antes de uma sincronização em larga escala.

Contribuições e Suporte
-----------------------
Pull requests e issues são bem-vindos. Para bugs ou sugestões, abra uma issue descrevendo passos para reproduzir e logs quando possível.

License
-------
GPL-3.0