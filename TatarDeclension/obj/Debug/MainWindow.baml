   M S B A M L   `   `   `  ����J  FTatarDeclension, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:TatarDeclensionTatarDeclension  5   	   O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089I2clr-namespace:System.Collections;assembly=mscorlibSystem.Collections 5   	        TatarDeclension.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:TatarDeclension     5   	   Jcollections2clr-namespace:System.Collections;assembly=mscorlib     5   	     � Title$4  .Склонение татарских слов��5	   	   $��715.2��6*   $	��480��69    � SizeToContent$ WidthAndHeight=�6E    � ShowInTaskbar .   � Topmost .  � 
ResizeMode$ NoResize=�6�    � WindowStartupLocation$ Manual=�6�   .��5
      � .+�5   
   �� -   5         ��Name$ IsInVowelHarmony��5      $O��IСлово подчиняется закону сингармонизма��6-   $
��Left=�6^   $��
105,65,0,0q�6y   $	��Top=�6�    d� 	IsChecked$
 TrueY�6�   $��16��6�   $	��258��6�   �� 5   
   -   5         ��Name$ Input��5      $
��Left=�6!   $��23��6<   $��
105,10,0,0q�6H   	 �� TextWrapping$
	 Wrap=�6\   $�� ��6p   $	��Top=�6x   $	��258��6�   �� 5   
   -   5        
 ��Name$
 Declense��5      $��Просклонять��6#   $
��Left=�69   $��
195,86,0,0q�6T   $	��Top=�6h   $��75��6�   6�   &   System.Windows.Controls.DataGrid  5   
   -   5        $��Table��5         AutoGenerateColumns .  $��10,110,10,10q�6<   $	��450��6R      
IsReadOnly .    Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ 
Килеш��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$	 150 6M   1  +System.Windows.Controls.DataGridBoundColumn   Binding 64   �� *Case+6M     5      $ Берлек сан��6%   $	 150 6V    69   �� *
Singular+6V     5      $ Күплек сан��6%   $	 150 6T    69   �� *Plural+6T   5      �� 5   
   -   5        $
 FirstSpecificLetter��5      $��ө��6.   $
��Left=�6:   $��	10,42,0,0q�6U   $	��Top=�6h   $��70��6�   6�   �� 5   
   -   5        $
 SecondSpecificLetter��5      $��ң��6/   $
��Left=�6;   $��	85,42,0,0q�6V   $	��Top=�6i   $��70��6�   6�   �� 5   
   -   5        $
 ThirdSpecificLetter��5      $��ү��6.   $
��Left=�6:   $��
160,42,0,0q�6U   $	��Top=�6i   $��70��6�   6�   �� 5   
   -   5        $
 FourthSpecificLetter��5      $��ә��6/   $��Right=�6;   $��
0,42,160,0q�6W   $	��Top=�6k   $��70��6�   6�   �� 5   
   -	   5        $
 FifthSpecificLetter��5      $��җ��6.   $��Right=�6:   $��	0,42,85,0q�6V   $	��Top=�6i   $��70��6�   6�   �� 5   
   -
   5        $
 SixthSpecificLetter��5      $��һ��6.   $��Right=�6:   $��	0,42,10,0q�6V   $	��Top=�6i   $��70��6�   6�   5      5      