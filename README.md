# bitrippr
This project is an experiment in encryption. 

<h4>On Encryption</h4>
- Encrypts files using other files as symmetric keys.  Each file uses a seperate piece of the 'key' file for encryption, so no piece is using the same key.
- Breaks up the encrypted bytes into seperate 'X number of' files. 
- Scatters the broken up parts to randomized places in the user's hard drive.

<h4>On Decryption</h4>
- Decrypts the 'header' file using the same symmetric key file. This contains the location for the first encrypted piece.
- Decrypts the rest of the files, as each file points to the next file in a linked-list sort of manner.  
- Cleans up the scattered pieces, and forms the unencrypted file back to it's original state. 
