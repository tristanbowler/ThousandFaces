/*
[*README*]

RPG Character Creation Pack
Lylek Games

Thank you for purchasing this asset!

[*RENDERING*]
This package was presented with the use of post-processing effects. To replicate the appearance displayed
in the product images, download the Post-Processing Stack available for free by Unity Technologies,
located here: https://assetstore.unity.com/packages/essentials/post-processing-stack-83912

The Post-Processing Profile we used in the demo scene(s) is included in the RPGCharacter>PostProcessingProfile folder.
After importing the Post-Processing Stack you may attach the Post Processing Behaviour script to the MainCamera in the scene,
and then add the Post-Processing Profile.

[*GETTING STARTED*]
To get started you can find the ready-to use character prefabs in the Resources folder.
Both the rpgcharacter_skin, and rpgcharacter_armored prefabs are ready for use and set up
with basic movement.
	
In the demo, we use the rpgcharacter_skin prefab, and a method of instantiating and destroying
armors via script instead of toggling components on and off. However, if you wish to toggle
armors instead, we have included an "rpgcharacter_armored" prefab in each of the character's
Resources folder.

To use the existing customization options, please open the CharacterCreation demo scene in the
Scenes folder. From there, you can pick from the available options to customize your character,
and add your own scripts to load the character into your scene. You may also use the demo scene
to generate characters and store them as prefabs, with the use of the CombineSkinnedMeshes script.
For further information please adhere to the Readme Combine file.

You can also customize characters in the Editor. Please refer to the Readme Update 1.3.2 for
more information.

[*MOVEMENT*]
With the current movement setup, your characters may run around using WASD, and toggle between
walking and running by holding SHIFT. Press the Tab key to toggle between combat and standard
movement states. Use key numbers 1 through 8 to toggle emotes.

You can find all animations, and the AnimatorController, in the RPGCharacter/Animations folder.

[*COMBINE MESHES*]
For information on combining meshes in game please refer to the Readme Combine file.

[*UPDATE 1.3.2 | CHARACTER ARMORED CUSTOMIZATIONS*]
This script was designed to allow customizations in the Editor, rather than having to use the demo scene.
To use this script, first drag and drop the RPGCharacter_armored prefab(s) into your scene. Note that this script will only be useful on such character gameObjects that contain
multiple armor meshes, (like the RPGCharacter_armored prefabs).

Go ahead and drag-and-drop the CharacterArmoredCustomizations script onto your RPGCharacter_arrmored gameObject. You may notice that you can't quite do anything yet! First we require the directory path
of the RPGCharacter folder. (Normally it may reside in the Assets/ folder, however if you have moved it, please specify this new path.) Next, defind what race and gender this chracter is, so we can
acquire the proper resources. Then press Update Character!

You should now have many customization options in front of you. The script has acquire all child character meshes, armrors, and hair styles, and has located the appropriate hair and skin materials
acording to the character's specified race and gender. You can press the drop down arrow on Character Resources to view all acquired customizations.

Feel free to drag the slider bars around! The characters will update in real time. When you have got your desired look for you character you may press 'Permanetly Combine Meshes'. This will use the
CombineSkinMeshesTextureAtlas script to combine your character into a single mesh, as well as remove all unused, hidden meshes, and the CharacterArmoredCustomizations script itself.

To save the character, view the CombineSkinMeshesTextureAtlas script and press Save Data. Do not press Combine Meshes or Disable Meshes, they will not work!

[*UPDATE 1.3.3 | CHARACTER PROPERTIES*]
In this update we reconfigured some scripts to be more flexible for use outside of the provided demo. You can find the primary scripts
in the Scripts/Character folder.

The CharacterProperties.cs script is now more reliable for storage and reference of your character's Skin Meshes, Armor Meshes, and Skin, Eye, and Hair Materials.
Along with the EquippmentSwap script, you can now easily modify character properties through your own means simply by calling a function, and providing the
appropriate parameter(s). For instance, you can swap armors in your game by referencing the CharacterProperties script, and calling the function EquippedNewArmor(),
while providing an armor mesh gameObject (such as a prefab) and specifying the armor type. You can remove (destroy), hide, and show armor types as well.

Movement components have also been removed from all RPGCharacter_skin prefabs. This is to prevent all generated characters from having a camera and player
movement script. Instead, you can now easily add movement components to your desired character(s) by selecting the character from the hierarchy and selecting
'Setup Player Movement' from the Tools > Lylek Games > RPG Character menu.

[*SUPPORT*]
We do our best to make our assets as user-friendly as possible; please, by all means, do not hesitate to send us an email if you have any questions or comments!
support@lylekgames.com, or visit http://www.lylekgames.com/contacts

**Please leave a rating and review! Even a small review may help immensely with prioritizing updates.**
(Assets with few and infrequent reviews/ratings tend to have less of a priority and my be late or miss-out on crucial compatibility updates, or even be depricated.)
Thank you! =)

*******************************************************************************************

Website
http://www.lylekgames.com/

Support
http://www.lylekgames.com/contacts
*/
