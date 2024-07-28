@echo off
macshift -i "Ethernet 1" 000C294B5E46
netsh diag ping loopback
macshift -i "Ethernet 4" 000C294B5E3C
netsh diag ping loopback
netsh diag ping loopback
netsh diag ping loopback
netsh interface ipv4 set address name="Ethernet 1" static 192.168.1.12 255.255.255.0
netsh interface ipv4 set address name="Ethernet 4" static 192.168.1.112 255.255.255.0