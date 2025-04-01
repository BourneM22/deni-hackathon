import 'package:deni_hackathon/constants/colors_constants.dart';
import 'package:deni_hackathon/screens/auth/forgot-password/forgot_password_controller.dart';
import 'package:deni_hackathon/widgets/deni_style.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';

class ForgotPasswordDialog extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return GetBuilder<ForgotPasswordController>(
        init: ForgotPasswordController(),
        builder: (ForgotPasswordController controller) {
          return Dialog(
            shape: RoundedRectangleBorder(
              borderRadius: BorderRadius.circular(20),
            ),
            backgroundColor: ColorsConstants.trueWhiteColor,
            child: Padding(
              padding: const EdgeInsets.all(20),
              child: SingleChildScrollView(
                child: Column(
                  children: [
                    Text(
                      'Forgot Password',
                      style: deniStyle(
                        fontSize: 24,
                        fontFamily: 'Afacad',
                        fontWeight: FontWeight.bold,
                        color: ColorsConstants.darkerPrimaryColor,
                      ),
                    ),
                    SizedBox(height: 10),
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
                          style: const TextStyle(
                            fontSize: 14,
                            color: ColorsConstants.darkGreyColor,
                            fontWeight: FontWeight.bold,
                          ),
                          controller: controller.nameTFController,
                          decoration: InputDecoration(
                            fillColor: Colors.white,
                            contentPadding:
                                const EdgeInsets.symmetric(horizontal: 10.0),
                            focusedBorder: OutlineInputBorder(
                              borderSide: BorderSide(
                                  color: ColorsConstants.darkGreyColor),
                              borderRadius: BorderRadius.circular(10.0),
                            ),
                            enabledBorder: OutlineInputBorder(
                              borderSide: BorderSide(
                                color: ColorsConstants.mediumGreyColor
                                    .withOpacity(0.4),
                              ),
                              borderRadius: BorderRadius.circular(10.0),
                            ),
                          ),
                          keyboardType: TextInputType.text,
                          // onChanged: controller.onChangeUsername,
                        ),
                        SizedBox(height: 20),
                        ElevatedButton(
                          style: ElevatedButton.styleFrom(
                            backgroundColor: ColorsConstants.primaryColor,
                            minimumSize: Size(double.infinity, 50),
                            shape: RoundedRectangleBorder(
                              borderRadius: BorderRadius.circular(8),
                            ),
                          ),
                          child: Text(
                            'Submit',
                            style: deniStyle(
                              fontSize: 16,
                              color: ColorsConstants.trueWhiteColor,
                              fontWeight: FontWeight.bold,
                            ),
                          ),
                          onPressed: () {},
                        ),
                        Container(
                          padding: const EdgeInsets.only(top: 4.0),
                          width: double.infinity,
                          child: Row(
                            mainAxisAlignment: MainAxisAlignment.end,
                            children: [
                              Text(
                                'Check your email for a password reset link!',
                                style: deniStyle(
                                  fontSize: 12,
                                  color: ColorsConstants.mediumGreyColor,
                                  fontWeight: FontWeight.bold,
                                ),
                              ),
                            ],
                          ),
                        ),
                      ],
                    )
                  ],
                ),
              ),
            ),
          );
        });
  }
}
