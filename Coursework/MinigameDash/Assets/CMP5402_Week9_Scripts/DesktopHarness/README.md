# Desktop harness for the Week 9 platform logic

Not part of the Unity project; Unity ignores it (the C# files are named `.cs.txt`). It compiles the real
`Scripts/Platform` and `Scripts/Progress` code against hand-written stand-ins for Unity, with a real JSON serialiser,
and drives the platform-independent logic through `NullPlatformServices` used as a controllable fake. With the .NET 8
SDK installed:

    cd DesktopHarness
    dotnet run

`PlatformHub` is configured with `ConfigureForTesting` (no scene, no Awake); its `OnApplicationFocus`,
`OnApplicationPause` and `Update` are called by reflection to test the flush triggers. `PendingSubmissions` writes real
files in a temp folder. The process exit code is non-zero if any check fails.

Scenarios (37 checks, all must print PASS):

A. `PlatformIds` resolves both store columns, falls back to the logical id for the Null back end, and returns null
   for an empty column or an unknown id; the hub logs a warning for an unknown id and queues and sends nothing
B. queue de-duplication: a score keeps the best (lowest when `lowerIsBetter`), progress keeps the highest absolute
   count, an unlock is kept once; the file holds the entries with their flags, a `.bak` is kept, no temp files remain,
   and a fresh instance reloads everything
C. declined sign-in: three submissions wait on disk with nothing sent; a later sign-in flushes them in order, each
   exactly once, and the file is empty afterwards
D. send failures: one attempt per trigger, parked after five, skipped while parked even when the back end works,
   `RetryParked` plus a trigger sends it; a better score replaces a failed entry and resets its attempts
E. a half-written temp file from a simulated crash is ignored; a corrupt main file falls back to the `.bak` with one
   warning; no files at all is an empty queue
F. the tracker: `first_clear` once and `speed_run` under 30 s, both times submitted and the local best kept, `hunter`
   reported in batches of ten as an absolute count, a save requested per event, `world_one` at level 10, and `Resync`
   re-sending three unlocks, the exact counter and the best time without touching local state
G. flush triggers: `OnApplicationFocus(true)`, `OnApplicationPause(false)`, and the reachability poll firing only when
   the network returns
H. a back end that throws is caught, counted as a failed attempt, and does not leave the queue stuck in flight

What this does NOT prove: anything about the live Google Play Games Services, Game Center, Unity Authentication or
Unity Leaderboards SDKs, the Play Games and Game Center adapters (they compile against stand-ins but never run here),
or that the stand-in signatures match the real packages in every detail. The Play Games and GameKit names were
checked against the plug-ins' own source (see SOURCES.md); the Unity Gaming Services names against Unity's
documentation. A simulated crash is a stray temp file, not a process kill mid-write.
