import 'package:flutter/material.dart';
import 'package:fluttertoast/fluttertoast.dart';
import 'package:get/get.dart';
import 'package:intl/intl.dart';

import '../../api/api_auth.dart';


class ProfileController extends GetxController {
  final nameTFController = TextEditingController();
  String gender = '';
  var birthDate = Rxn<DateTime>();
  final emailTFController = TextEditingController();
  final passwordTFController = TextEditingController();
  
  bool isLoading = false;
  bool isError = false;
  bool isEditMode = false;

  @override
  void onInit() {
    isLoading = true;
    isError = false;
    super.onInit();
    getProfile();
  }

  @override
  void dispose() {
    super.dispose();
  }

  Future<void> getProfile() async {
    try {
      final response = await apiAuth.getProfileData();

      nameTFController.text = response.name!;
      gender = response.gender == 'M' ? 'Male' : 'Female';
      birthDate.value = DateTime.parse(response.birthDate!);
      emailTFController.text = response.email!;
      passwordTFController.text = '**********';
      isLoading = false;
      isError = false;
      update();
    } catch (e) {
      Fluttertoast.showToast(
        msg: 'Failed to fetch profile data: $e',
        toastLength: Toast.LENGTH_LONG,
        gravity: ToastGravity.BOTTOM,
        backgroundColor: Colors.red,
        textColor: Colors.white,
        fontSize: 16.0,
      );
    }
  }

  Future<void> onSaveProfile() async {
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
        'password': passwordTFController.text == '**********'
            ? null
            : passwordTFController.text,
      };

      final response = await apiAuth.updateProfile(data);

      if (response == true) {
        Fluttertoast.showToast(
          msg: 'Profile updated successfully',
          toastLength: Toast.LENGTH_LONG,
          gravity: ToastGravity.BOTTOM,
          backgroundColor: Colors.green,
          textColor: Colors.white,
          fontSize: 16.0,
        );
        isEditMode = false;
        update();
      } else {
        Fluttertoast.showToast(
          msg: 'Failed to update profile data',
          toastLength: Toast.LENGTH_LONG,
          gravity: ToastGravity.BOTTOM,
          backgroundColor: Colors.red,
          textColor: Colors.white,
          fontSize: 16.0,
        );
      }
      
    } catch (e) {
      Fluttertoast.showToast(
        msg: 'Failed to update profile data: $e',
        toastLength: Toast.LENGTH_LONG,
        gravity: ToastGravity.BOTTOM,
        backgroundColor: Colors.red,
        textColor: Colors.white,
        fontSize: 16.0,
      );
    }
  }

  void toggleEditMode() {
    isEditMode = !isEditMode;
    update();
  }
}