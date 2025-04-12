import 'package:flutter/material.dart';
import 'package:fluttertoast/fluttertoast.dart';
import 'package:get/get.dart';
import 'package:intl/intl.dart';

import '../../../api/api_auth.dart';
import '../../../constants/colors_constants.dart';
import '../../../utils/logger.dart';
import '../login/login_dialog.dart';

class RegisterController extends GetxController {
  final nameTFController = TextEditingController();
  String gender = '';
  var birthDate = Rxn<DateTime>();
  final emailTFController = TextEditingController();
  final passwordTFController = TextEditingController();

  @override
  void onInit() {
    super.onInit();
  }

  @override
  void dispose() {
    nameTFController.dispose();
    emailTFController.dispose();
    passwordTFController.dispose();
    birthDate.close();
    super.dispose();
  }

  Future<void> register() async {
    try {
      String? formattedBirthDate =
          birthDate.value != null
              ? DateFormat('yyyy-MM-dd').format(birthDate.value!)
              : null;

      final data = {
        'name': nameTFController.text,
        'email': emailTFController.text,
        'gender': gender == 'Male' ? 'M' : 'F',
        'birthDate': formattedBirthDate,
        'password': passwordTFController.text,
      };

      final response = await apiAuth.signIn(data);

      if (response == true) {
        Fluttertoast.showToast(
          msg: 'Register Success, please login',
          toastLength: Toast.LENGTH_LONG,
          gravity: ToastGravity.BOTTOM,
          backgroundColor: ColorsConstants.blackToastColor,
          textColor: ColorsConstants.trueWhiteColor,
          fontSize: 16.0,
        );
        closeAndShowModal(Get.context!);
      } else if (response == false) {
        Fluttertoast.showToast(
          msg: 'Email already used',
          toastLength: Toast.LENGTH_LONG,
          gravity: ToastGravity.BOTTOM,
          backgroundColor: ColorsConstants.blackToastColor,
          textColor: ColorsConstants.trueWhiteColor,
          fontSize: 16.0,
        );
      } else {
        Fluttertoast.showToast(
          msg: 'Email have been used',
          toastLength: Toast.LENGTH_LONG,
          gravity: ToastGravity.BOTTOM,
          backgroundColor: ColorsConstants.blackToastColor,
          textColor: ColorsConstants.trueWhiteColor,
          fontSize: 16.0,
        );
      }
    } catch (e) {
      log.e('Error: $e');
      Fluttertoast.showToast(
        msg: 'An error occurred. Please try again.',
        toastLength: Toast.LENGTH_LONG,
        gravity: ToastGravity.BOTTOM,
        backgroundColor: ColorsConstants.blackToastColor,
        textColor: ColorsConstants.trueWhiteColor,
        fontSize: 16.0,
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
        return LoginDialog();
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
