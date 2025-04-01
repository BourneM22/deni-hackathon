import 'package:deni_hackathon/screens/auth/welcome/welcome_screen.dart';
import 'package:get/get.dart';
import 'route_name.dart';

class AppPages {
  static final pages = [
    /* Auth */
    GetPage(
      name: AuthRoute.welcome,
      page: () => WelcomeScreen(),
    ),

    /* Home */
    // GetPage(
    //   name: HomeRoute.dashboard,
    //   page: () => const MainNavigationScreen(),
    // ),
  ];
}
