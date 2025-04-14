import 'package:deni_hackathon/constants/colors_constants.dart';
import 'package:deni_hackathon/screens/main/notes/notes_controller.dart';
import 'package:flutter/material.dart';
import 'package:flutter_svg/svg.dart';
import 'package:get/get.dart';

import '../../../constants/assets_constants.dart';
import '../../../widgets/deni_style.dart';

class AddNoteScreen extends StatelessWidget {
  const AddNoteScreen({super.key});

  @override
  Widget build(BuildContext context) {
    return GetBuilder<NotesController>(
      init: NotesController(),
      builder: (NotesController controller) {
        return Scaffold(
          backgroundColor: ColorsConstants.backgroundColor,
          floatingActionButton: Row(
          mainAxisAlignment: MainAxisAlignment.end,
          children: [
            // Delete Button
            Container(
              margin: const EdgeInsets.only(right: 12),
              decoration: BoxDecoration(
                shape: BoxShape.circle,
                border: Border.all(
                  color: ColorsConstants.darkerPrimaryColor,
                  width: 1,
                ),
              ),
              child: FloatingActionButton(
                onPressed: () {
                  // controller.toggleEditMode(); // Exit edit mode
                  Get.back(); // Close the screen
                },
                backgroundColor: Colors.transparent,
                elevation: 0,
                child: SvgPicture.asset(
                  AssetConstants.deleteIcon,
                  height: 24,
                  color: ColorsConstants.darkerPrimaryColor,
                ),
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
                  await controller.onSaveNotes();
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
        ),
          // APP BAR
          appBar: AppBar(
            backgroundColor: ColorsConstants.backgroundColor,
            leading: IconButton(
              icon: const Icon(Icons.arrow_back),
              onPressed: () {
                Get.back();
              },
            ),
            title: Text(
              'Notes',

              style: deniStyle(
                fontWeight: FontWeight.w700,
                fontSize: 16,
                color: ColorsConstants.mediumGreyColor,
                fontFamily: 'manrope'
              ),
            ),
          ),

          // MAIN
          body: Container(
            // styles
            decoration: BoxDecoration(
              color: ColorsConstants.darkerBackgroundColor,
              border: const Border(
                bottom: BorderSide(
                  color: Colors.grey,
                  width: 1.0,
                ),
              ),
              borderRadius: const BorderRadius.only(
                topLeft: Radius.circular(20),
                topRight: Radius.circular(20),
              ),
            ),

            // child
            child: Column(
              children: [
                // TITLE & TYPE INPUT
                Container(
                  // style
                  padding: EdgeInsets.only(
                    top: 20, bottom: 12
                  ),
            
                  decoration: const BoxDecoration(
                    border: Border(
                      bottom: BorderSide(
                        color: ColorsConstants.lightGreyColor, // Change to your preferred color
                        width: 1.0,
                      ),
                    ),
                  ),

                  // child
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                  
                    children: [
                      Container(
                        padding: EdgeInsets.only(
                          left: 20, right: 20
                        ),
                  
                        child: TextFormField(
                          controller: controller.titleTFController,
                          decoration: InputDecoration(
                            hintText: 'Enter a title...',
                            hintStyle: deniStyle(
                              color: ColorsConstants.mediumGreyColor,
                              fontSize: 24.0,
                              fontFamily: 'Afacad',
                              fontWeight: FontWeight.w500
                            )
                          ),
                        ),
                      ),
                  
                      SizedBox(height: 12),
                  
                      Container(
                        padding: EdgeInsets.only(
                          top: 0, bottom: 0, left: 20, right: 20
                        ),
                  
                        child: Container(
                          padding: EdgeInsets.only(
                            top: 0, bottom: 0, left: 12, right: 12
                          ),
                          decoration: BoxDecoration(
                            color: ColorsConstants.darkestBackgroundColor,
                            borderRadius: BorderRadius.circular(1000),
                          ),
                        
                          child: DropdownButton(
                            dropdownColor: ColorsConstants.darkestBackgroundColor,
                            underline: SizedBox(),
                            iconSize: 20,
                        
                            value: 'All',
                            items: <String>['All', 'Option 1', 'Option 2', 'Option 3'].map((String value) {
                              return DropdownMenuItem<String>(
                                value: value,
                                child: Text(
                                  value,
                                  style: deniStyle(
                                    fontFamily: 'Afacad',
                                    color: ColorsConstants.mediumGreyColor,
                                    fontSize: 16.0,
                                    fontWeight: FontWeight.w500
                                  ),
                                ),
                              );
                            }).toList(),
                        
                            onChanged: (newValue) {
                              // setState(() {
                              //   selectedValue = newValue!;
                              // });
                            },
                          )
                        ),
                      )
                    ],
                  ),
                ),
            
                // CONTENT INPUT
                Expanded(
                  child: TextField(
                    controller: controller.contentTFController,
                    decoration: InputDecoration(
                      hintText: 'Write your note here...',
                      hintStyle: deniStyle(
                        color: ColorsConstants.mediumGreyColor,
                        fontSize: 16.0,
                        fontFamily: 'Manrope',
                        fontWeight: FontWeight.w500
                      ),
                      
                      border: InputBorder.none,
                      enabledBorder: InputBorder.none,
                      focusedBorder: InputBorder.none,
                      contentPadding: const EdgeInsets.all(16),
                    ),
                    keyboardType: TextInputType.multiline,
                    maxLines: null,
                  ),
                ),
            
              ],
            ),
          )
        );
      },
    );
  }
}
