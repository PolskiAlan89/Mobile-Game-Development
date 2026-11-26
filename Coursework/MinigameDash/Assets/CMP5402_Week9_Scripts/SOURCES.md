# Week 9 sources

The same list appears on slides 27 and 28 of the lecture. Accessed November 2026. The plug-ins and the Unity Gaming Services APIs change often: check the live pages before building.

1. [Social API (deprecated)](https://docs.unity.com/en-us/engine/6000.3/manual/platform-specific/iphone/ios-developing/net-social-api). Unity Manual 6000.3, slides 03, 13, 25.
2. [Set up Google Play Games for Unity and authenticate](https://developer.android.com/games/pgs/unity/unity-start). Android Developers, slides 03, 06, 09, 10, 13, 15, 24.
3. [Achievements in Unity games](https://developer.android.com/games/pgs/unity/achievements). Android Developers, slides 10, 19.
4. [Leaderboards in Unity games](https://developer.android.com/games/pgs/unity/leaderboards). Android Developers, slide 10.
5. [Google Play Games sign-in](https://docs.unity.com/en-us/authentication/platform-signin/google-play-games). Unity Authentication documentation, slides 05, 07, 09.
6. [Apple Game Center sign-in](https://docs.unity.com/en-us/authentication/platform-signin/apple-game-center). Unity Authentication documentation, slides 05, 07.
7. [Apple Unity Plug-ins (Apple.Core, Apple.GameKit)](https://github.com/apple/unityplugins). GitHub, apple/unityplugins, slides 06, 11, 12, 13, 15, 19, 23, 24.
8. [Get started with Leaderboards](https://docs.unity.com/en-us/leaderboards/get-started). Unity Gaming Services documentation, slide 21.
9. [Leaderboard features](https://docs.unity.com/ugs/en-us/manual/leaderboards/manual/features). Unity Gaming Services documentation, slide 22.
10. [Add a new score (Unity SDK tutorial)](https://docs.unity.com/ugs/en-us/manual/leaderboards/manual/tutorials/unity-sdk). Unity Gaming Services documentation, slide 21.
11. [Access Leaderboards via Cloud Code](https://docs.unity.com/ugs/en-us/manual/leaderboards/manual/access-cloud-code). Unity Gaming Services documentation, slides 21, 22.
12. [Children's code guidance and resources](https://ico.org.uk/for-organisations/uk-gdpr-guidance-and-resources/childrens-information/childrens-code-guidance-and-resources/). Information Commissioner's Office, slide 23.
13. [Google Play Families Policies](https://support.google.com/googleplay/android-developer/answer/9893335). Play Console Help, slides 06, 23.
14. [Apple Unity Plug-ins: Quick Start Guide](https://github.com/apple/unityplugins/blob/main/Documentation/Quickstart.md). GitHub, apple/unityplugins (Documentation/Quickstart.md), slides 11, 12.

Also consulted for API names (not cited on a slide): the Google Play Games plugin source, `PlayGamesPlatform.cs` in github.com/playgameservices/play-games-plugin-for-unity (SetStepsAtLeast, RequestServerSideAccess, SignInStatus), and the Apple Unity Plug-ins GameKit source under plug-ins/Apple.GameKit (GKGameCenterViewControllerState is a top-level enum in Apple.GameKit; GKAccessPoint.Trigger(state); GKIdentityVerificationResponse.Timestamp is a UInt64).
