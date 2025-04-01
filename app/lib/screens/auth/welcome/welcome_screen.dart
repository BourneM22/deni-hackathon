import 'package:deni_hackathon/constants/assets_constants.dart';
import 'package:deni_hackathon/constants/colors_constants.dart';
import 'package:deni_hackathon/screens/auth/welcome/welcome_controller.dart';
import 'package:deni_hackathon/widgets/deni_style.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';

import '../register/register_dialog.dart';

class WelcomeScreen extends StatelessWidget {
  const WelcomeScreen({super.key});

  @override
  Widget build(BuildContext context) {
    return GetBuilder<WelcomeController>(
        init: WelcomeController(),
        builder: (WelcomeController controller) {
          return Scaffold(
            body: Stack(
              fit: StackFit.expand,
              children: [
                Image.asset(
                  AssetConstants.loginBackground,
                  fit: BoxFit.cover,
                ),
                Container(
                  decoration: BoxDecoration(
                    gradient: LinearGradient(
                      begin: Alignment.topCenter,
                      end: Alignment.bottomCenter,
                      colors: [
                        ColorsConstants.trueWhiteColor.withOpacity(0.4),
                        ColorsConstants.trueBlackColor.withOpacity(0.4),
                      ],
                      stops: [0.0, 0.65],
                    ),
                  ),
                ),
                Padding(
                  padding:
                      const EdgeInsets.symmetric(horizontal: 20, vertical: 50),
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    mainAxisAlignment: MainAxisAlignment.end,
                    children: [
                      Text(
                        '"Connect Clearly,\nLive Fully."',
                        style: deniStyle(
                          fontSize: 30,
                          color: ColorsConstants.trueWhiteColor,
                          fontWeight: FontWeight.bold,
                        ),
                      ),
                      SizedBox(height: 20),
                      ElevatedButton.icon(
                        style: ElevatedButton.styleFrom(
                          minimumSize: Size(double.infinity, 50),
                          backgroundColor: Colors.white,
                          foregroundColor: Colors.black,
                          shape: RoundedRectangleBorder(
                            borderRadius: BorderRadius.circular(8),
                          ),
                        ),
                        icon: Image.asset(
                          AssetConstants.google,
                          height: 20,
                        ),
                        label: Text('Register / Login with Google'),
                        onPressed: () {
                          _handleGoogleLogin(context);
                        },
                      ),
                      SizedBox(height: 10),
                      ElevatedButton.icon(
                        style: ElevatedButton.styleFrom(
                          minimumSize: Size(double.infinity, 50),
                          backgroundColor: Colors.white,
                          foregroundColor: Colors.black,
                          shape: RoundedRectangleBorder(
                            borderRadius: BorderRadius.circular(8),
                          ),
                        ),
                        icon: Icon(Icons.email, color: Colors.black),
                        label: Text('Register / Login with Email'),
                        onPressed: () {
                          showGeneralDialog(
                            context: context,
                            barrierDismissible: true,
                            barrierLabel: MaterialLocalizations.of(context)
                                .modalBarrierDismissLabel,
                            barrierColor:
                                Colors.black54,
                            transitionDuration: const Duration(
                                milliseconds: 300),
                            pageBuilder:
                                (context, animation, secondaryAnimation) {
                              return RegisterDialog();
                            },
                            transitionBuilder: (context, animation,
                                secondaryAnimation, child) {
                              const begin =
                                  Offset(0.0, 1.0);
                              const end =
                                  Offset(0.0, 0.0);
                              const curve = Curves.easeInOut;

                              var tween = Tween(begin: begin, end: end)
                                  .chain(CurveTween(curve: curve));
                              var offsetAnimation = animation.drive(tween);

                              return SlideTransition(
                                position: offsetAnimation,
                                child: child,
                              );
                            },
                          );
                        },
                      ),
                      SizedBox(height: 20),
                      Center(
                        child: Column(
                          children: [
                            Text(
                              'By signing up, I agree to the',
                              textAlign: TextAlign.center,
                              style: deniStyle(
                                color: ColorsConstants.trueWhiteColor,
                                fontSize: 12,
                              ),
                            ),
                            Text(
                              'Terms & Conditions, Privacy Policy, and Disclaimer.',
                              textAlign: TextAlign.center,
                              style: deniStyle(
                                  color: ColorsConstants.trueWhiteColor,
                                  fontSize: 12,
                                  fontWeight: FontWeight.bold),
                            )
                          ],
                        ),
                      ),
                    ],
                  ),
                ),
              ],
            ),
          );
        });
  }

  void _handleGoogleLogin(BuildContext context) {
    // Implement Google login logic
    // After successful login, you might navigate to home screen:
    // Navigator.pushReplacement(context, MaterialPageRoute(builder: (context) => HomeScreen()));
  }

  void _handleEmailLogin(BuildContext context) {
    // Implement Email login logic
    // You might show a dialog or navigate to email login screen:
    // Navigator.push(context, MaterialPageRoute(builder: (context) => EmailLoginScreen()));
  }
}
