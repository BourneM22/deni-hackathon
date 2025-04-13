import 'package:deni_hackathon/constants/assets_constants.dart';
import 'package:deni_hackathon/screens/profile/profile_controller.dart';
import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:flutter_svg/svg.dart';
import 'package:get/get.dart';

import '../../../constants/colors_constants.dart';
import '../../../widgets/deni_style.dart';

class ProfileScreen extends StatelessWidget {
  const ProfileScreen({super.key});

  @override
  Widget build(BuildContext context) {
    return GetBuilder<ProfileController>(
      init: ProfileController(),
      builder: (ProfileController controller) {
        return Scaffold(
          backgroundColor: ColorsConstants.baseColor,
          appBar: AppBar(
            backgroundColor: ColorsConstants.baseColor,
            title: Text(
              'Edit Profile',
              style: deniStyle(
                fontSize: 16,
                fontWeight: FontWeight.bold,
                color: ColorsConstants.mediumGreyColor,
              ),
            ),
            leading: Padding(
              padding: const EdgeInsets.only(left: 16.0),
              child: IconButton(
                icon: SvgPicture.asset(
                  AssetConstants.backIcon,
                  height: 24,
                  color: ColorsConstants.mediumGreyColor,
                ),
                onPressed: () {
                  Navigator.pop(context);
                },
              ),
            ),
          ),
          floatingActionButton: buildFloatingActionButton(controller),
          body:
              controller.isLoading == true
                  ? const Center(
                    child: CircularProgressIndicator(
                      color: ColorsConstants.primaryLightColor,
                    ),
                  )
                  : controller.isError == true
                  ? Center(
                    child: Text(
                      'Error loading profile',
                      style: deniStyle(
                        fontSize: 16,
                        color: ColorsConstants.mediumGreyColor,
                      ),
                    ),
                  )
                  : RefreshIndicator(
                    onRefresh: () async {
                      controller.onInit();
                    },
                    child: SingleChildScrollView(
                      physics: const AlwaysScrollableScrollPhysics(),
                      child: Column(
                        children: [
                          Stack(
                            alignment: Alignment.bottomRight,
                            children: [
                              CircleAvatar(
                                radius: 60,
                                backgroundImage: AssetImage(
                                  AssetConstants.defaultProfile,
                                ),
                                backgroundColor: ColorsConstants.whiteColor,
                              ),
                              FloatingActionButton.small(
                                onPressed: () {},
                                backgroundColor: const Color(0xFF7B2D26),
                                shape: const CircleBorder(),
                                elevation: 0,
                                child: const Icon(
                                  Icons.edit,
                                  color: Colors.white,
                                  size: 18,
                                ),
                              ),
                            ],
                          ),
                          const SizedBox(height: 20),
                          Container(
                            height: MediaQuery.of(context).size.height * 0.75,
                            padding: const EdgeInsets.symmetric(
                              vertical: 20,
                              horizontal: 16,
                            ),
                            decoration: BoxDecoration(
                              color: ColorsConstants.whiteColor,
                              borderRadius: BorderRadius.circular(20),
                            ),
                            child: Column(
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
                                  enabled: controller.isEditMode,
                                  style: deniStyle(
                                    fontSize: 14,
                                    color: ColorsConstants.darkGreyColor,
                                    fontWeight: FontWeight.bold,
                                  ),
                                  controller: controller.nameTFController,
                                  decoration: InputDecoration(
                                    filled: true,
                                    fillColor: ColorsConstants.darkerWhiteColor,
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
                                    disabledBorder: OutlineInputBorder(
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
                                AbsorbPointer(
                                  absorbing: !controller.isEditMode,
                                  child: DropdownButtonFormField(
                                    value: controller.gender,
                                    style: deniStyle(
                                      fontSize: 14,
                                      color: ColorsConstants.darkGreyColor,
                                      fontWeight: FontWeight.bold,
                                    ),
                                    decoration: InputDecoration(
                                      filled: true,
                                      fillColor:
                                          ColorsConstants.darkerWhiteColor,
                                      contentPadding:
                                          const EdgeInsets.symmetric(
                                            horizontal: 10.0,
                                          ),
                                      focusedBorder: OutlineInputBorder(
                                        borderSide: BorderSide(
                                          color: ColorsConstants.darkGreyColor,
                                        ),
                                        borderRadius: BorderRadius.circular(
                                          10.0,
                                        ),
                                      ),
                                      enabledBorder: OutlineInputBorder(
                                        borderSide: BorderSide(
                                          color:
                                              ColorsConstants.transparentColor,
                                        ),
                                        borderRadius: BorderRadius.circular(
                                          10.0,
                                        ),
                                      ),
                                      disabledBorder: OutlineInputBorder(
                                        borderSide: BorderSide(
                                          color:
                                              ColorsConstants.transparentColor,
                                        ),
                                        borderRadius: BorderRadius.circular(
                                          10.0,
                                        ),
                                      ),
                                      hintText: 'Pick your gender',
                                    ),
                                    iconEnabledColor:
                                        ColorsConstants.mediumGreyColor,
                                    dropdownColor: ColorsConstants.baseColor,
                                    items: [
                                      DropdownMenuItem(
                                        value: 'Male',
                                        child: Text('Male'),
                                      ),
                                      DropdownMenuItem(
                                        value: 'Female',
                                        child: Text('Female'),
                                      ),
                                    ],
                                    onChanged: (value) {
                                      controller.gender = value.toString();
                                    },
                                  ),
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
                                  onTap:
                                      controller.isEditMode
                                          ? () async {
                                            DateTime? pickedDate =
                                                await showDatePicker(
                                                  context: context,
                                                  initialDate:
                                                      controller
                                                          .birthDate
                                                          .value ??
                                                      DateTime.now(),
                                                  firstDate: DateTime(1900),
                                                  lastDate: DateTime.now(),
                                                );

                                            if (pickedDate != null) {
                                              controller.birthDate.value =
                                                  pickedDate;
                                              controller.update();
                                            }
                                          }
                                          : null,
                                  child: AbsorbPointer(
                                    child: TextField(
                                      enabled: controller.isEditMode,
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
                                                : ColorsConstants
                                                    .mediumGreyColor,
                                        fontWeight: FontWeight.bold,
                                      ),
                                      decoration: InputDecoration(
                                        filled: true,
                                        fillColor:
                                            ColorsConstants.darkerWhiteColor,
                                        contentPadding:
                                            const EdgeInsets.symmetric(
                                              horizontal: 10.0,
                                            ),
                                        focusedBorder: OutlineInputBorder(
                                          borderSide: BorderSide(
                                            color:
                                                ColorsConstants.darkGreyColor,
                                          ),
                                          borderRadius: BorderRadius.circular(
                                            10.0,
                                          ),
                                        ),
                                        enabledBorder: OutlineInputBorder(
                                          borderSide: BorderSide(
                                            color:
                                                ColorsConstants
                                                    .transparentColor,
                                          ),
                                          borderRadius: BorderRadius.circular(
                                            10.0,
                                          ),
                                        ),
                                        disabledBorder: OutlineInputBorder(
                                          borderSide: BorderSide(
                                            color:
                                                ColorsConstants
                                                    .transparentColor,
                                          ),
                                          borderRadius: BorderRadius.circular(
                                            10.0,
                                          ),
                                        ),
                                        hintText: 'Pick a date',
                                        hintStyle: deniStyle(
                                          fontSize: 14,
                                          color:
                                              ColorsConstants.mediumGreyColor,
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
                                  enabled: controller.isEditMode,
                                  style: deniStyle(
                                    fontSize: 14,
                                    color: ColorsConstants.darkGreyColor,
                                    fontWeight: FontWeight.bold,
                                  ),
                                  controller: controller.emailTFController,
                                  decoration: InputDecoration(
                                    filled: true,
                                    fillColor: ColorsConstants.darkerWhiteColor,
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
                                    disabledBorder: OutlineInputBorder(
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
                                  enabled: controller.isEditMode,
                                  style: deniStyle(
                                    fontSize: 14,
                                    color: ColorsConstants.darkGreyColor,
                                    fontWeight: FontWeight.bold,
                                  ),
                                  controller: controller.passwordTFController,
                                  decoration: InputDecoration(
                                    filled: true,
                                    fillColor: ColorsConstants.darkerWhiteColor,
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
                                    disabledBorder: OutlineInputBorder(
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
                              ],
                            ),
                          ),
                        ],
                      ),
                    ),
                  ),
        );
      },
    );
  }

  Widget buildFloatingActionButton(ProfileController controller) {
    return controller.isEditMode
        ? Row(
          mainAxisAlignment: MainAxisAlignment.end,
          children: [
            // Cancel Button
            Container(
              margin: const EdgeInsets.only(right: 12),
              decoration: BoxDecoration(
                shape: BoxShape.circle,
                gradient: LinearGradient(
                  begin: Alignment.topCenter,
                  end: Alignment.bottomCenter,
                  colors: [Color(0xFF6C2727), Color(0xFF000000)],
                ),
              ),
              child: FloatingActionButton(
                onPressed: () {
                  controller.toggleEditMode(); // Exit edit mode
                },
                backgroundColor: Colors.transparent,
                elevation: 0,
                child: const Icon(Icons.close, color: Colors.white),
              ),
            ),
            // Save Button
            Container(
              decoration: BoxDecoration(
                shape: BoxShape.circle,
                gradient: LinearGradient(
                  begin: Alignment.topCenter,
                  end: Alignment.bottomCenter,
                  colors: [Color(0xFFAB4141), Color(0xFF451B1B)],
                ),
              ),
              child: FloatingActionButton(
                onPressed: () async {
                  await controller.onSaveProfile();
                  FocusScope.of(Get.context!).unfocus(); // Dismiss keyboard
                },
                backgroundColor: Colors.transparent,
                elevation: 0,
                child: SvgPicture.asset(
                  AssetConstants.saveIcon,
                  height: 24,
                  color: Colors.white,
                ),
              ),
            ),
          ],
        )
        : Container(
          decoration: BoxDecoration(
            shape: BoxShape.circle,
            gradient: LinearGradient(
              begin: Alignment.topCenter,
              end: Alignment.bottomCenter,
              colors: [Color(0xFFAB4141), Color(0xFF451B1B)],
            ),
          ),
          child: FloatingActionButton(
            onPressed: () {
              controller.toggleEditMode();
            },
            backgroundColor: Colors.transparent,
            elevation: 0,
            child: SvgPicture.asset(
              AssetConstants.editIcon,
              height: 24,
              color: Colors.white,
            ),
          ),
        );
  }
}
