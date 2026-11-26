# CMP5402 Week 9: Leaderboards, Achievements & Platform Services scripts

Unity 6.3 LTS (6000.3). Drag the whole `CMP5402_Week9_Scripts` folder into your project's `Assets` folder. Everything is
in the `CMP5402.Week9` namespace. The pack compiles in any project: the store adapters are behind
`GPGS_INSTALLED` (Google Play Games plugin) and `APPLE_GAMEKIT_INSTALLED` (Apple Unity Plug-ins), the Unity
Authentication linking behind `UGS_AUTH_INSTALLED`, and the Unity Leaderboards helper behind
`UGS_LEADERBOARDS_INSTALLED`. Without any of them the game runs on `NullPlatformServices`: every unlock and score is
queued, logged and "sent" to a fake, which is exactly what the lab needs.

## Set-up

Core (no accounts, no device):
1. Assets > Create > CMP5402 > Platform IDs. Add your achievements (logical id, and `totalSteps` for incremental ones)
   and leaderboards (logical id, tick `lowerIsBetter` for time boards). Leave the store columns empty for now.
2. An empty `Platform` GameObject in the first scene with `PlatformHub` (assign the asset) and, in a test scene,
   `PlatformTestMenu`. The hub survives scene loads and tries one silent sign-in at launch.
3. Raise events from your gameplay through `AchievementTracker` (or copy its shape into your own class).

Google Play Games Services (Android):
1. Play Console > Play Games Services: create the game, add achievements and leaderboards, copy each ID into the
   `playGamesId` column. Add every tester account: unpublished games only sign in testers.
2. Import the Google Play Games plugin for Unity (.unitypackage, v11 or later). Play Console > Get resources, then
   Window > Google Play Games > Setup > Android Setup, paste the XML, Setup.
3. Add `GPGS_INSTALLED` to Project Settings > Player > Other Settings > Scripting Define Symbols (Android).

Apple Game Center (iOS):
1. App Store Connect: enable Game Center, add achievements and leaderboards, copy the IDs into `gameCenterId`.
2. Clone github.com/apple/unityplugins, run `python3 build.py`, import the Apple.Core and Apple.GameKit tarballs
   through the Package Manager. The repository README lists iOS 15.6 as the minimum: set Player Settings to match.
3. Add `APPLE_GAMEKIT_INSTALLED` to the iOS define symbols. After the first Xcode export check that the target has the
   Game Center capability under Signing & Capabilities. Sandbox tester accounts go in Settings > Game Center on the
   device.

Unity Authentication linking (both):
1. Authentication package (`com.unity.services.authentication`, 2.4.0 or later), project linked in
   Project Settings > Services, and Week 7's bootstrap signing in anonymously at launch.
2. Dashboard > Authentication > ID Providers: Google Play Games (web client ID and secret from the OAuth client
   created for the Play Console project) and/or Apple Game Center (bundle ID).
3. Add `UGS_AUTH_INSTALLED`; call `AccountLinker.LinkCurrentPlatformAsync(PlatformHub.Instance)` from a Link button
   and handle `AlreadyLinkedElsewhere` by asking the player before `SignInWithCurrentPlatformAsync`.

Unity Leaderboards (cross-platform board):
1. Leaderboards package (`com.unity.services.leaderboards`). Dashboard > Development > Products > Leaderboards: create
   the board with its sort order and score strategy; put its id in the asset's `ugsId` column.
2. Add `UGS_LEADERBOARDS_INSTALLED`; `UgsLeaderboard.SubmitAsync / TopAsync / MeAsync`. This is the client-submits
   path; for a board that matters, submit from Cloud Code and deny the client in Access Control.

Wiring to your save: `ProgressRecord` (counters plus the unlocked list) is meant to become one field of your Week 7
`SaveData`, so it is versioned, written atomically and cloud-synced with everything else; pass your save request as the
tracker's `save` callback. Call `tracker.Resync()` after a cloud restore or a first sign-in on a new device: every
report is absolute, so re-sending is harmless. The queue file lives in `persistentDataPath/platform/` and is separate
from the save on purpose: it is outbound work, not truth.

## Scripts

| Script | Slide | What it does |
|---|---|---|
| `Platform/IPlatformServices.cs` | The interface | The one seam: `SignIn(interactive)`, `SubmitScore`, `ReportProgress(steps, totalSteps)` (absolute, so retries cannot double-count), `ShowLeaderboard`, `ShowAchievements`; callbacks only, no vendor types, platform ids only. |
| `Platform/NullPlatformServices.cs` | The interface | The Editor, harness and no-plug-in back end. Records every call; `SimulateSignedOut`, `SimulateFailures` and `SimulateUnderage` rehearse the matrix rows. |
| `Platform/PlatformIds.cs` | Logical IDs | ScriptableObject mapping logical ids to the Play Console and App Store Connect ids, with `totalSteps`, `lowerIsBetter` and an optional UGS id. Unknown ids resolve to null; the Null column is the logical id. |
| `Platform/PendingSubmissions.cs` | The offline queue | Disk-first queue with Week 7's file rules (temp then `File.Replace`, `.bak`, version). De-duplicates by kind and id keeping the best score or highest count; one submission in flight; attempts counted and parked at five; `RetryParked`, `Clear`. |
| `Platform/PlatformHub.cs` | Logical IDs | The only object gameplay calls. Picks the back end by platform and define symbol in `Awake`, silent sign-in in `Start`, `UnlockAchievement` / `ReportProgress` / `SubmitScore` with logical ids, flush on sign-in, resume and reachability (poll, or call `OnReachabilityRegained` from Week 7's NetProbe). |
| `Platform/PlayGamesServices.cs` | Play Games: sign in, submit, show | Adapter over `PlayGamesPlatform`: `Authenticate` / `ManuallyAuthenticate` with `SignInStatus`, `ReportScore`, `ReportProgress(100)` for standard and `SetStepsAtLeast` for incremental achievements, the native UIs, and `RequestServerSideAccess` for the auth code. `GPGS_INSTALLED` only. |
| `Platform/GameCenterServices.cs` | Game Center: sign in, submit, show | Adapter over Apple.GameKit: `GKLocalPlayer.Authenticate`, `IsUnderage`, `LoadLeaderboards` then `SubmitScore`, `GKAchievement` percent reports, `GKAccessPoint.Shared.Trigger(state)`, `FetchItems` for Unity Authentication. Converts async/await and `GameKitException` to callbacks. `APPLE_GAMEKIT_INSTALLED` only. |
| `Progress/AchievementTracker.cs` | AchievementTracker | Gameplay events in, logical ids out. `ProgressRecord` holds the counters and unlocked list (fold it into your save). Unlocks locally first, batches incremental progress, submits times, `Resync`. Example rules to replace with yours. |
| `Leaderboards/UgsLeaderboard.cs` | Cross-platform boards | `AddPlayerScoreAsync` with metadata, `GetScoresAsync` top list, `GetPlayerScoreAsync`. Client-submits path, with the Cloud Code route pointed to. `UGS_LEADERBOARDS_INSTALLED` only. |
| `Identity/AccountLinker.cs` | Linking a platform account | Anonymous player first, then `LinkWithGooglePlayGamesAsync(authCode)` or `LinkWithAppleGameCenterAsync(signature, teamPlayerId, publicKeyURL, salt, timestamp)`; `AccountAlreadyLinked` reported, never merged; `SignInWith...` for the player's choice. `UGS_AUTH_INSTALLED` plus an adapter define. |
| `Tools/PlatformTestMenu.cs` | The platform services test matrix | On-screen buttons: silent and interactive sign-in, refuse sign-in, go offline, underage, drive the tracker (level, enemies), unknown id, flush, retry parked, clear, resync, show the native UIs, and the live queue listing with attempts. Three-finger tap or F4 hides it. |

## Evidence

The eight-row matrix on slide 24 plus a screenshot per row. Rows 1 to 5 can be rehearsed in the Editor with the test
menu (and the queue file before and after); rows 6 to 8 need devices with tester accounts on the stores. The harness
output (`dotnet run` in `DesktopHarness/`) counts as evidence for the queue and the tracker.