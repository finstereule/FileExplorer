   M S B A M L   `   `   `  ����?  ;lab1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:lab1lab1  5   	       
lab1.Login   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   $localclr-namespace:lab1     5   	     � Title$  LoginWindow��5   	   $	��290��6   $	��290��6*   $	��290��6:   $	��170��6J   $	��300��6Y   $	��400��6e   .��5	      � -   5        $��	LoginGrid��5	       � RowDefinitions 5
   
   �� 5      $��70 �6   �� 5      $��70 �6   �� 5      $��20��6   $��30��6,   $	��31* �6;   �� 5      $��37 �6   �� 5      $��20��6   $��30��6,   $	��29* �6;    � ColumnDefinitions 5   
   �� 5      �� 5      .+�5   
   �� $��0��6   $��2��6"   .+�5      �� $��Username��6   $��5q�6'   $
��Left=�62   $	��Top=�6M   �� 5      -   5        $��Username��5   h   $��
10,0,10,10q�6   $��Stretch=�6*   $��23��6H    �� TextWrapping$
 Wrap=�6T   $	��Top=�6x   ��6�   �� *
Username+ �� UpdateSourceTrigger$ PropertyChanged=�5      �� 5   
   $��1��6   $��2��6"   .+�5      �� $��Password��6   $��5q�6'   $
��Left=�62   $	��Top=�6M   G� 5      -   5        $��Password��5   p   $��
10,0,10,10q�6   $��Stretch=�6.   $��23��6L   $	��Top=�6X   6�   5      �� 5   
   $��2��6   $��0��6   $��Sign up��6.   $��Center=�6@   $��Center=�6]   $��79��6x    J� RenderTransformOrigin$ 0.056,0.417&�6�   $��	19,0,48,0q�6�   $��31��6�   ��6�   �� *SignUpCommand+6�   �� 5   
   $��2��6   $��1��6   $��Log in��6.   $��Center=�6?   $��Center=�6\   $��86��6w   $��	38,0,22,0q�6�   $��31��6�   ��6�   �� *SignInCommand+6�   �� 5!   
   $��3��6   $��2��6"   $��126,0q�66   �� 5#   
   $��4��6   $��1��6   $��Cancel��6.   $��Center=�6?   $��Center=�6\   $��86��6w   $��	38,1,22,0q�6�   $��30��6�   ��6�   �� *CloseCommand+6�   5$      5%      