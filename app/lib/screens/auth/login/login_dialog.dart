import 'package:deni_hackathon/constants/colors_constants.dart';
import 'package:deni_hackathon/screens/auth/login/login_controller.dart';
import 'package:deni_hackathon/widgets/deni_style.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';

class LoginDialog extends StatelessWidget {
  const LoginDialog({super.key});

  @override
  Widget build(BuildContext context) {
    return GetBuilder<LoginController>(
      init: LoginController(),
      builder: (LoginController controller) {
        return Dialog(
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(20),
          ),
          backgroundColor: ColorsConstants.trueWhiteColor,
          child: Padding(
            padding: const EdgeInsets.all(20),
            child: SingleChildScrollView(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(
                    'Welcome Back!',
                    style: deniStyle(
                      fontSize: 24,
                      fontFamily: 'Afacad',
                      color: ColorsConstants.darkGreyColor,
                    ),
                  ),
                  // Divider
                  Container(
                    height: 1.0,
                    width: double.infinity,
                    color: ColorsConstants.mediumGreyColor,
                    margin: const EdgeInsets.symmetric(vertical: 10.0),
                  ),
                  Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Text(
                        'Email',
                        style: deniStyle(
                          fontSize: 14,
                          color: ColorsConstants.mediumGreyColor,
                          fontWeight: FontWeight.bold,
                        ),
                      ),
                      SizedBox(height: 4.0),
                      TextField(
                        style: deniStyle(
                          fontSize: 14,
                          color: ColorsConstants.darkGreyColor,
                          fontWeight: FontWeight.bold,
                        ),
                        controller: controller.emailTFController,
                        decoration: InputDecoration(
                          filled: true,
                          fillColor: ColorsConstants.whiteColor,
                          contentPadding: const EdgeInsets.symmetric(
                            horizontal: 10.0,
                          ),
                          focusedBorder: OutlineInputBorder(
                            borderSide: BorderSide(
                              color: ColorsConstants.darkGreyColor,
                            ),
                            borderRadius: BorderRadius.circular(10.0),
                          ),
                          enabledBorder: OutlineInputBorder(
                            borderSide: BorderSide(
                              color: ColorsConstants.transparentColor,
                            ),
                            borderRadius: BorderRadius.circular(10.0),
                          ),
                          hintText: 'Enter your email...',
                        ),
                        keyboardType: TextInputType.text,
                        // onChanged: controller.onChangeUsername,
                      ),
                      SizedBox(height: 10),
                      Text(
                        'Password',
                        style: deniStyle(
                          fontSize: 14,
                          color: ColorsConstants.mediumGreyColor,
                          fontWeight: FontWeight.bold,
                        ),
                      ),
                      SizedBox(height: 4.0),
                      TextField(
                        style: deniStyle(
                          fontSize: 14,
                          color: ColorsConstants.darkGreyColor,
                          fontWeight: FontWeight.bold,
                        ),
                        controller: controller.passwordTFController,
                        decoration: InputDecoration(
                          filled: true,
                          fillColor: ColorsConstants.whiteColor,
                          contentPadding: const EdgeInsets.symmetric(
                            horizontal: 10.0,
                          ),
                          focusedBorder: OutlineInputBorder(
                            borderSide: BorderSide(
                              color: ColorsConstants.darkGreyColor,
                            ),
                            borderRadius: BorderRadius.circular(10.0),
                          ),
                          enabledBorder: OutlineInputBorder(
                            borderSide: BorderSide(
                              color: ColorsConstants.transparentColor,
                            ),
                            borderRadius: BorderRadius.circular(10.0),
                          ),
                          suffixIcon: Icon(
                            Icons.visibility_off,
                            size: 18.0,
                            color: ColorsConstants.mediumGreyColor,
                          ),
                          hintText: 'Enter your password...',
                        ),
                        obscureText: true,
                      ),
                      SizedBox(height: 20),
                      ElevatedButton(
                        style: ElevatedButton.styleFrom(
                          backgroundColor: ColorsConstants.primaryLightColor,
                          minimumSize: Size(double.infinity, 50),
                          shape: RoundedRectangleBorder(
                            borderRadius: BorderRadius.circular(8),
                          ),
                        ),
                        child:
                            controller.isLoading == true
                                ? const CircularProgressIndicator(
                                  color: ColorsConstants.trueWhiteColor,
                                )
                                : const Text(
                                  'Login',
                                  style: TextStyle(
                                    fontSize: 16,
                                    color: ColorsConstants.primaryColor,
                                    fontWeight: FontWeight.bold,
                                  ),
                                ),
                        onPressed: () {
                          controller.login();
                        },
                      ),
                      Row(
                        mainAxisAlignment: MainAxisAlignment.end,
                        children: [
                          TextButton(
                            onPressed: () {
                              controller.closeAndShowModal(context);
                            },
                            child: Row(
                              children: [
                                Text(
                                  'Forgot password?',
                                  style: deniStyle(
                                    fontSize: 14,
                                    color: ColorsConstants.mediumGreyColor,
                                    fontWeight: FontWeight.bold,
                                  ),
                                ),
                                Text(
                                  ' Reset',
                                  style: deniStyle(
                                    fontSize: 14,
                                    color: ColorsConstants.primaryColor,
                                    fontWeight: FontWeight.bold,
                                  ),
                                ),
                              ],
                            ),
                          ),
                        ],
                      ),
                    ],
                  ),
                ],
              ),
            ),
          ),
        );
      },
    );
  }
}
