   M S B A M L   `   `   `  ����F  BNikki_Pulls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:Nikki_PullsNikki_Pulls  5   	       Nikki_Pulls.History   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:Nikki_Pulls     5   	     � Title$  History��5   	   $	��450��6   $	��800��6&   
������  � AllowsTransparency$
 True��5
   	    � WindowStyle$
 None=�5   	    � WindowStartupLocation$ CenterScreen=�5   	    � Icon$ 
/heart.ico��6.    � 	Resources 5       �� 
TargetType �� Property �� Value3  -System.Windows.Media.Effects.DropShadowEffect	   Color
   
BlurRadius   ShadowDepth �� 
TargetType �� CornerRadius&   System.Windows.Controls.DataGrid=  7System.Windows.Controls.Primitives.DataGridColumnHeader)  #System.Windows.Controls.DataGridRow��%
  '��       '  �    '  b    '  }    �� 5   
   " ��6   .�5      ��  � ��$ 5q�6'   �� 5       � ��$
 10,5q�6(   �� 5       � ��$ SemiBold"�6+   �� 5       � �� �	#FFB48FC26+   �� 5       � �� �	#FFF8BBD06+   �� 5       � �� �	#FFF48FB16,   �� 5       � ��$ 1q�60   �� 5       �@�Cursor$
 Hand��6'   �� 5       �@J�Effect 5        5      $	 	#40B48FC2��6'   $
 7Q�69   $ 2Q�6H   5      �� 5       � �� 5      �� 5      " ��6&   .R�5      �� #��z��6!   #��z��5    !   #��z��5!   !   $ 8��5"   !   ._�5#      �� $��Center=�6/   $��Center=�6L   5$      5%      5'      5(      �� 5)   
   "  6   .�5*      ��  � �� �	#FFF3E5F56+   �� 5+       � �� �	#FF7C4DFF6+   �� 5,       � �� �	#FFCE93D86,   �� 5-       � ��$ 1q�60   �� 5.       �@ RowBackground �	#FFF8BBD06.   �� 5/        �@ AlternatingRowBackground �	#FFFCE4EC69   �� 50       � ��$ 14(�6)   �� 51       �@ GridLinesVisibility$ 
Horizontal=�64   52      �� 53   
   "  6   .�54      ��  � �� �	#FFF8BBD06+   �� 55       � �� �	#FFB48FC26+   �� 56       � ��$
 Bold"�6+   �� 57       � ��$ 15(�6)   �� 58       � �� �	#FFF48FB16,   �� 59       � ��$ 0,0,1,1q�60   5:      �� 5;   
   "  6   .�5<      ��  � ��$ 14(�6)   �� 5=       � �� �	#FF7C4DFF6+   �� 5>       � �� �	#FFF3E5F56+   �� 5?       � �� �	#FFF8BBD06,   �� 5@       � ��$ 0,0,0,1q�60   �� 5A       �@J�Effect 5B        5C      $	 	#40B48FC2��6'   $
 3Q�69   $ 1Q�6H   5E      5F      5G      .��5H      �� ���	#FF000000$��1q�6%   $ 18��69   ���	#FFF8EAF6._�5J   
   � .+�5K        -   5          Name$ PullsDataGrid��5K         AutoGenerateColumns$
 True��6.   $��10,10,10,42q�6{   �� 5L      -   5        $��btnClose��5L      $��Close��6%   $��Right=�65   $��Bottom=�6Q   $	��100��6l   $��	0,0,20,15q�6x   6�   �� 5M      -   5         ��Name$ btnClean��5M      $��Clean��6'   $
��Left=�67   $��Bottom=�6R   $	��100��6m   $��	20,0,0,15q�6y   6�   5N      5O      5P      