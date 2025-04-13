import 'package:deni_hackathon/screens/auth/welcome/welcome_screen.dart';
import 'package:deni_hackathon/screens/main/emergency/emergency_screen.dart';
import 'package:deni_hackathon/screens/main/main_screen.dart';
import 'package:deni_hackathon/screens/main/notes/notes_screen.dart';
import 'package:get/get.dart';
import '../screens/profile/profile_screen.dart';
import 'route_name.dart';

class AppPages {
  static final pages = [
    /* Auth */
    GetPage(
      name: AuthRoute.welcome,
      page: () => const WelcomeScreen(),
    ),

    /* Main */
    GetPage(
      name: MainRoute.main,
      page: () => const MainScreen(),
    ),
    GetPage(
      name: MainRoute.emergency,
      page: () => const EmergencyScreen(),
    ),
    GetPage(
      name: MainRoute.notes,
      page: () => const NotesScreen(),
    ),

    /* Profile */
    GetPage(
      name: ProfileRoute.profile,
      page: () => const ProfileScreen(),
    ),
  ];
}
