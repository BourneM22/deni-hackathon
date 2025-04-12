import 'package:deni_hackathon/routes/route_name.dart';
import 'package:flutter/material.dart';
import 'package:fluttertoast/fluttertoast.dart';
import 'package:get/get.dart';
import 'package:shared_preferences/shared_preferences.dart';

import '../../../api/api_auth.dart';
import '../../../constants/colors_constants.dart';
import '../forgot-password/forgot_password_dialog.dart';

class LoginController extends GetxController {
  final emailTFController = TextEditingController();
  final passwordTFController = TextEditingController();

  @override
  void onInit() {
    super.onInit();
  }

  @override
  void dispose() {
    super.dispose();
  }

  Future<void> login() async {
    try {
      final data = {
        'email': emailTFController.text,
        'password': passwordTFController.text,
      };

      final response = await apiAuth.login(data);

      // Set the token in shared preferences
      final prefs = await SharedPreferences.getInstance();
      await prefs.setString('token', response.accessToken!);
      await prefs.setString('tokenType', response.tokenType!);
      await prefs.setInt('expiresInMinutes', response.expiresInMinutes!);
      
      Get.offAllNamed(MainRoute.main);
    } catch (e) {
      Fluttertoast.showToast(
        msg: "Email and password doesn't match!",
        toastLength: Toast.LENGTH_LONG,
        gravity: ToastGravity.CENTER,
        backgroundColor: ColorsConstants.blackToastColor,
        textColor: ColorsConstants.trueWhiteColor,
        fontSize: 12.0,
      );
    }
  }

  void closeAndShowModal(BuildContext context) {
    Navigator.pop(context);
    showGeneralDialog(
      context: context,
      barrierDismissible: true,
      barrierLabel: MaterialLocalizations.of(context).modalBarrierDismissLabel,
      barrierColor: Colors.black54,
      transitionDuration: const Duration(milliseconds: 300),
      pageBuilder: (context, animation, secondaryAnimation) {
        return ForgotPasswordDialog();
      },
      transitionBuilder: (context, animation, secondaryAnimation, child) {
        const begin = Offset(0.0, 1.0);
        const end = Offset(0.0, 0.0);
        const curve = Curves.easeInOut;

        var tween = Tween(
          begin: begin,
          end: end,
        ).chain(CurveTween(curve: curve));
        var offsetAnimation = animation.drive(tween);

        return SlideTransition(position: offsetAnimation, child: child);
      },
    );
  }
}
