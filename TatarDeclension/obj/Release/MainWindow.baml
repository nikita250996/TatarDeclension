   M S B A M L   `   `   `  ���� J  FTatarDeclension, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:TatarDeclensionTatarDeclension  O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089I2clr-namespace:System.Collections;assembly=mscorlibSystem.Collections      TatarDeclension.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     /localclr-namespace:TatarDeclension     Jcollections2clr-namespace:System.Collections;assembly=mscorlib       � Title$4  .Склонение татарских слов��$��715.2��$	��480�� � SizeToContent$ WidthAndHeight=� � ShowInTaskbar .   � Topmost .  � 
ResizeMode$ NoResize=� � WindowStartupLocation$ Manual=�.��� .+��� -    ��Name$ IsInVowelHarmony��$O��IСлово подчиняется закону сингармонизма��$
��Left=�$��
105,65,0,0q�$	��Top=� d� 	IsChecked$
 TrueY�$��16��$	��258���� -    ��Name$ Input��$
��Left=�$��23��$��
105,10,0,0q�	 �� TextWrapping$
	 Wrap=�$�� ��$	��Top=�$	��258���� -   
 ��Name$
 Declense��$��Просклонять��$
��Left=�$��
195,86,0,0q�$	��Top=�$��75��&   System.Windows.Controls.DataGrid  -   $��Table��   AutoGenerateColumns .  $��10,110,10,10q�$	��450��   
IsReadOnly .    Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ 
Килеш��   Width5  /System.Windows.Controls.DataGridLengthConverter$	 150 1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *Case+  $ Берлек сан��$	 150  �� *
Singular+  $ Күплек сан��$	 150  �� *Plural+�� -   $
 FirstSpecificLetter��$��ө��$
��Left=�$��	10,42,0,0q�$	��Top=�$��70���� -   $
 SecondSpecificLetter��$��ң��$
��Left=�$��	85,42,0,0q�$	��Top=�$��70���� -   $
 ThirdSpecificLetter��$��ү��$
��Left=�$��
160,42,0,0q�$	��Top=�$��70���� -   $
 FourthSpecificLetter��$��ә��$��Right=�$��
0,42,160,0q�$	��Top=�$��70���� -	   $
 FifthSpecificLetter��$��җ��$��Right=�$��	0,42,85,0q�$	��Top=�$��70���� -
   $
 SixthSpecificLetter��$��һ��$��Right=�$��	0,42,10,0q�$	��Top=�$��70��