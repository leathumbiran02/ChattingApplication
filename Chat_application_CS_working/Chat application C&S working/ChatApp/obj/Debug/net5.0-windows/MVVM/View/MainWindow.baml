   M S B A M L   `   `   `  ����E  AChatClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:ChatAppChatApp  5          ChatApp.MainWindow   5      W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      'localclr-namespace:ChatApp     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5        � Title$  Chatting Application��5      $	��814��5	      $	��470��5
      ��5      �� 5   
    �� ImageSource$" /Images/chat-back-image.jpeg��6    j� Stretch$
 None=�6@   .��5      �  � ColumnDefinitions 5   
   �� 5      $	��200 �6   �� 5      .+�5   
   ]� .+�5      �� $��25��5      $��20,40,20,10q�5      ���	#FFEEEEEE$	��Top=�5      ��5      �� *
Username+ �� UpdateSourceTrigger$ PropertyChanged=��� 5      -   5         ��Name$ connect_btn��5       $��25��5!      $��20,20,20,20q�5"      $��Connect��5$      $	��Top=�5%      $��Arial)�5&      $��15(�5'      $��Normal$�5(      ��5#      �� *ConnectToServerCommand+��5)      �� 5*       �� 
TargetType" ��6&   .R�5+      ��  ��Name$ border��6!   #��z��61   ._�5,      ��  ��Name$ contentPresenter��5-   !   $��Center=�5.   !   $��Center=�5/   !   #��z��50   !   #��z��51   !   52      	 �� Triggers	 53      P� 54      
 P� Property
 � {� P� Value$ False��6;   . �55   "   ��  �� 
TargetName$ border��6)    �� Property � �� �� Value �#3C096C6S   �� 56   "    � �� �#EEEEEE6?   57      P� 58      
 � {�$
 True��6;   . �59   "   �� $ border��6)    � �� �#001C556S   �� 5:   "    � ��
 �����6?   5;      5=      5?      �� 5@      $��0,0,0,80q�5A      
������ $��0q�5C      $��Arial)�5D      $��15(�5E      
���������5G      �� *Users+��5H      �� 5I      .R�5J      �� ��6$   �� *
Username+5K      5M      5N      �� 5O   
   -   5        $��25��5P      $��20,385,20,20q�5Q      ���#001C55���	#FF282828$��Logout��5U      $��Bottom=�5V      $��Arial)�5W      $��15(�5X      $��Normal$�5Y      
�������5Z      �� 5[   
   $��1��6   .+�5\      �� $	��380��5]      
������ $��0q�5_      $��Arial)�5`      $��15(�5a      
���������5c      �� *
Messages+�� 5e      
������  �� Orientation$ 
Horizontal=�62   .+�5f      �� $	��545��5g      $��55��5h      $��Center=�5i      ���	#FFEEEEEE$��0q�5k      $��Arial)�5l      $��15(�5m      
���   ���5o      �� *	Message+$ PropertyChanged=��� 5p      -   5        $ send_btn��5q      $��55��5r      $
��Send��5t      $��Arial)�5u      $��15(�5v      ��5s      �� *SendMessageCommand+��5w      �� 5x      " ��6*   .R�5y      �� $ border��6%   #��z��65   ._�5z   "   �� $ contentPresenter��5{   %   $��Center=�5|   %   $��Center=�5}   %   #��z��5~   %   #��z��5   %   5�      	 5�      P� 5�   "   
 � {�$ False��6?   . �5�   &   �� $ border��6-    � �� �#3C096C6W   �� 5�   &    � �� �#EEEEEE6C   5�   #   P� 5�   "   
 � {�$
 True��6?   . �5�   &   �� $ border��6-    � �� �#001C556W   �� 5�   &    � ��
 �����6C   5�   #   5�      5�      5�      5�      5�      5�      