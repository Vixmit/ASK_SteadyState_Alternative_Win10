   M S B A M L   `   `   `  ���� \  XASK_SteadyState_Alternative_Win10, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullk:clr-namespace:ASK_SteadyState_Alternative.TreeView_control,ASK_SteadyState_Alternative.TreeView_control  =    7ASK_SteadyState_Alternative.TreeView_control.MainWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Kself:clr-namespace:ASK_SteadyState_Alternative.TreeView_control       � Title$%  TreeViewMultipleTemplatesSample��$	��480��$��720         ��.��� $��10q� � ColumnDefinitions �� $
��263* ��� $
��429* �.+�]� R� -   $��treeView��$��	8,30,0,10q� J� RenderTransformOrigin$ 0.319,0.478&� � 	Resources  �� DataType8   2ASK_SteadyState_Alternative.TreeView_control.Group �� ItemsSource �� Orientation7   1ASK_SteadyState_Alternative.TreeView_control.User��%_  (��       *, +)(�� �     *, +)�� "   �� *	Members+.R��� $ 
Horizontal=�.+��� $��Images/group.png��$��0,0,5,0q��� ���� *Name+�� $�� [��
����  ��� 
����  ����� *Members.Count+�� $��]��
����  ��� "  .R��� $ 
Horizontal=�.+��� $��/Images/user.png��$��0,0,5,0q��� ���� *Name+���� ����  �� Color$ 	#FF828790�� �� Opacity$ 0Q��� $��1��$��
10,30,0,10q�.@��� $��Informations��.��� 
������� �� $	��20* ��� $	��49* �.+��� -   	 ��Name$	 textBoxInformations��$
��Left=�$	��352��$��10,0,0,0q�$
��Wrap=�$	��Top=�$	��394��$��2���� -   
 ��Name$
 
editButton��$��2��$
��Edit��$
��Left=�$��30��$��10,0,0,0q�$��Bottom=�$��90��$y�Hidden=��� -   $
 deleteButton��$��2��$��Delete��$
��Left=�$��30��$��	110,0,0,0q�$��Bottom=�$��90��$y�Hidden=��� -   $
 addUserButton��$��2��$��Add new User��$
��Left=�$��30��$��	210,0,0,0q�$��Bottom=�$��90��$y�Hidden=�o� $��25��$	��Top=�$��2�����	#FF45C708���	#FF8CEE2B$��14(�.@�m� $��_File��.@�m� $��_Exit��m� $��_Group��.@�m� -   $��Add new group��