import 'package:deni_hackathon/constants/colors_constants.dart';
import 'package:deni_hackathon/screens/auth/login/login_dialog.dart';
import 'package:deni_hackathon/screens/auth/register/register_controller.dart';
import 'package:deni_hackathon/widgets/deni_style.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';

class RegisterDialog extends StatelessWidget {
  const RegisterDialog({super.key});

  @override
  Widget build(BuildContext context) {
    return GetBuilder<RegisterController>(
        init: RegisterController(),
        builder: (RegisterController controller) {
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
                      'Register',
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
                          'Name',
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
                        SizedBox(height: 10),
                        Text(
                          'Gender',
                          style: deniStyle(
                            fontSize: 14,
                            color: ColorsConstants.mediumGreyColor,
                            fontWeight: FontWeight.bold,
                          ),
                        ),
                        SizedBox(height: 4.0),
                        DropdownButtonFormField(
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
                          iconEnabledColor: ColorsConstants.mediumGreyColor,
                          items: [
                            DropdownMenuItem(
                                child: Text('Male'), value: 'Male'),
                            DropdownMenuItem(
                                child: Text('Female'), value: 'Female'),
                          ],
                          onChanged: (value) {},
                        ),
                        SizedBox(height: 10),
                        Text(
                          'Birth date',
                          style: deniStyle(
                            fontSize: 14,
                            color: ColorsConstants.mediumGreyColor,
                            fontWeight: FontWeight.bold,
                          ),
                        ),
                        SizedBox(height: 4.0),
                        TextField(
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
                            suffixIcon: Icon(
                              Icons.calendar_today,
                              size: 16.0,
                              color: ColorsConstants.mediumGreyColor,
                            ),
                          ),
                        ),
                        SizedBox(height: 10),
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
                            suffixIcon: Icon(
                              Icons.visibility_off,
                              size: 18.0,
                              color: ColorsConstants.mediumGreyColor,
                            ),
                          ),
                          obscureText: true,
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
                            'Sign Up',
                            style: deniStyle(
                              fontSize: 16,
                              color: ColorsConstants.trueWhiteColor,
                              fontWeight: FontWeight.bold,
                            ),
                          ),
                          onPressed: () {},
                        ),
                        Row(
                          mainAxisAlignment: MainAxisAlignment.end,
                          children: [
                            TextButton(
                              onPressed: () {
                                Navigator.pop(context);
                                showGeneralDialog(
                                  context: context,
                                  barrierDismissible: true,
                                  barrierLabel:
                                      MaterialLocalizations.of(context)
                                          .modalBarrierDismissLabel,
                                  barrierColor: Colors.black54,
                                  transitionDuration:
                                      const Duration(milliseconds: 300),
                                  pageBuilder:
                                      (context, animation, secondaryAnimation) {
                                    return LoginDialog();
                                  },
                                  transitionBuilder: (context, animation,
                                      secondaryAnimation, child) {
                                    const begin = Offset(0.0, 1.0);
                                    const end = Offset(0.0, 0.0);
                                    const curve = Curves.easeInOut;

                                    var tween = Tween(begin: begin, end: end)
                                        .chain(CurveTween(curve: curve));
                                    var offsetAnimation =
                                        animation.drive(tween);

                                    return SlideTransition(
                                      position: offsetAnimation,
                                      child: child,
                                    );
                                  },
                                );
                              },
                              child: Row(
                                children: [
                                  Text(
                                    'Already have an account?',
                                    style: deniStyle(
                                      fontSize: 14,
                                      color: ColorsConstants.mediumGreyColor,
                                      fontWeight: FontWeight.bold,
                                    ),
                                  ),
                                  Text(
                                    ' Login',
                                    style: deniStyle(
                                      fontSize: 14,
                                      color: ColorsConstants.primaryColor,
                                      fontWeight: FontWeight.bold,
                                    ),
                                  )
                                ],
                              ),
                            ),
                          ],
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
