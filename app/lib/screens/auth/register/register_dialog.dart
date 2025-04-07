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
          backgroundColor: ColorsConstants.baseColor,
          child: Padding(
            padding: const EdgeInsets.all(20),
            child: SingleChildScrollView(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(
                    'Create an Account',
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
                        'Name',
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
                        controller: controller.nameTFController,
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
                          hintText: 'Enter your name...',
                          // hintStyle: deniStyle(
                          //   fontSize: 14,
                          //   color: ColorsConstants.mediumGreyColor,
                          // ),
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
                        style: deniStyle(
                          fontSize: 14,
                          color: ColorsConstants.darkGreyColor,
                          fontWeight: FontWeight.bold,
                        ),
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
                          hintText: 'Pick your gender',
                        ),
                        iconEnabledColor: ColorsConstants.mediumGreyColor,
                        dropdownColor: ColorsConstants.baseColor,
                        items: [
                          DropdownMenuItem(value: 'Male', child: Text('Male')),
                          DropdownMenuItem(
                            value: 'Female',
                            child: Text('Female'),
                          ),
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
                      GestureDetector(
                        onTap: () async {
                          DateTime? pickedDate = await showDatePicker(
                            context: context,
                            initialDate: DateTime.now(),
                            firstDate: DateTime(1900),
                            lastDate: DateTime.now(),
                            builder: (BuildContext context, Widget? child) {
                              return Theme(
                                data: Theme.of(context).copyWith(
                                  colorScheme: ColorScheme.light(
                                    primary:
                                        ColorsConstants
                                            .primaryLightColor,
                                    onPrimary:
                                        ColorsConstants
                                            .whiteColor,
                                    onSurface:
                                        ColorsConstants
                                            .darkGreyColor,
                                  ),
                                  textButtonTheme: TextButtonThemeData(
                                    style: TextButton.styleFrom(
                                      foregroundColor:
                                          ColorsConstants
                                              .primaryLightColor,
                                    ),
                                  ),
                                ),
                                child: child!,
                              );
                            },
                          );

                          if (pickedDate != null) {
                            controller.birthDate.value =
                                pickedDate;
                          }
                          controller.update();
                        },
                        child: AbsorbPointer(
                          child: TextField(
                            controller: TextEditingController(
                              text:
                                  controller.birthDate.value != null
                                      ? '${controller.birthDate.value!.day}/${controller.birthDate.value!.month}/${controller.birthDate.value!.year}'
                                      : '',
                            ),
                            style: deniStyle(
                              fontSize: 14,
                              color:
                                  controller.birthDate.value != null
                                      ? ColorsConstants.darkGreyColor
                                      : ColorsConstants.mediumGreyColor,
                              fontWeight: FontWeight.bold,
                            ),
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
                              hintText: 'Pick a date',
                              hintStyle: deniStyle(
                                fontSize: 14,
                                color: ColorsConstants.mediumGreyColor,
                              ),
                            ),
                            readOnly: true, // Prevents manual input
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
                        child: Text(
                          'Sign Up',
                          style: deniStyle(
                            fontSize: 16,
                            color: ColorsConstants.primaryColor,
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
                                    MaterialLocalizations.of(
                                      context,
                                    ).modalBarrierDismissLabel,
                                barrierColor: Colors.black54,
                                transitionDuration: const Duration(
                                  milliseconds: 300,
                                ),
                                pageBuilder: (
                                  context,
                                  animation,
                                  secondaryAnimation,
                                ) {
                                  return LoginDialog();
                                },
                                transitionBuilder: (
                                  context,
                                  animation,
                                  secondaryAnimation,
                                  child,
                                ) {
                                  const begin = Offset(0.0, 1.0);
                                  const end = Offset(0.0, 0.0);
                                  const curve = Curves.easeInOut;

                                  var tween = Tween(
                                    begin: begin,
                                    end: end,
                                  ).chain(CurveTween(curve: curve));
                                  var offsetAnimation = animation.drive(tween);

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
