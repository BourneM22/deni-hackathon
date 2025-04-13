import 'package:flutter/material.dart';

import '../../../constants/colors_constants.dart';
import '../../../widgets/deni_style.dart';

class NotesScreen extends StatelessWidget {
  const NotesScreen({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(

      body: Container(
        color: ColorsConstants.backgroundColor,

        child: Column(
          children: [
            // HEADER
            Container(
              padding: const EdgeInsets.only(
                top: 60, left: 20, right: 20, bottom: 20
              ),

              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,

                children: [
                  Text(
                    'Notes',

                    style: deniStyle(
                      fontSize: 24,
                      fontFamily: 'Afacad',
                      fontWeight: FontWeight.w700,
                      color: ColorsConstants.darkerPrimaryColor
                    ),
                  ),
                  
                  Text(
                    'Task you’ve added & all notes you write here will be used by Deni to generate customize response.',

                    style: deniStyle(
                      fontSize: 14,
                      fontWeight: FontWeight.w300
                    )
                  )
                ],
              ),
            ),
            
            // MAIN
            Expanded(child: Container(
              decoration: BoxDecoration(
                color: ColorsConstants.darkerBackgroundColor,
                borderRadius: BorderRadius.only(
                  topLeft: Radius.circular(20.0),
                  topRight: Radius.circular(20.0),
                ),
              ),

              child: Column(
                children: [
                  // SEARCH FILTER
                  Container(
                    padding: const EdgeInsets.only(
                      top: 12, left: 12, right: 12, bottom: 8,
                    ),
                    decoration: BoxDecoration(
                      border: Border(
                        bottom: BorderSide(
                          color: ColorsConstants.lightGreyColor,
                          width: 1.0,
                        ),
                      ),
                    ),

                    child: Form(
                      child: Column(
                        children: [
                          // SEARCH INPUT
                          Row(
                            children: [
                              Expanded(child: 
                                TextFormField(
                                  decoration: InputDecoration(
                                    hintText: 'Find a note...',
                                    hintStyle: deniStyle(
                                      color: ColorsConstants.mediumGreyColor,
                                      fontSize: 14.0,
                                      fontFamily: 'Manrope',
                                      fontWeight: FontWeight.w500
                                    ),
                                    prefixIcon: Icon(Icons.search),
                                    border: OutlineInputBorder(
                                      borderRadius: BorderRadius.circular(1000)
                                    ),
                                  ),
                                )
                              ),
                              SizedBox(width: 16),
                              PopupMenuButton(itemBuilder: (context) => [])
                            ],
                          ),

                          SizedBox(height: 12),

                          // FILTER OPTIONS
                          Row(
                            children: [
                              Container(
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
                              )
                            ],
                          )
                        ],
                      ),
                    )
                  )
                ],
              ),
            ))
          ],
        ),
      ),

    );
  }
}
