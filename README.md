# EasyCrackMe solution

This is a solution for the CrackMe challenge [EasyCrackMe](https://crackmes.one/crackme/64563b8733c5d43938912ee9) made by [BeginnerCracker123](https://crackmes.one/user/BeginnerCracker123) on [crackmes.one](https://crackmes.one). As the name implies, this is a difficulty level 1 CrackMe challenge.

## Tools used:
- [JetBrains Rider](https://www.jetbrains.com/rider/)
- [x64dbg](https://github.com/x64dbg/x64dbg)

## Instructions:
After attaching x64dbg, search for string references in the main module. There we can see interesting strings such as:
- "Correct :)" (Which appears briefly upon entering the correct password, before the program closes)
- "Incorrect :(" (Which appears if an invalid password is entered, before prompting the user to try again)
- "12345" (Which is the correct password, not very secure!)

We double click on the string which says "Incorrect :(" and look at the instructions in that memory space. x64dbg conveniently shows us that there is a jne (jump if not equal) instruction which takes us to the string I mentioned. By simply patching that instruction and replacing it with a jmp (unconditional jump) and setting the relative offset for the jump to the part of the program which is responsible for handling the correct password input, we can effectively bypass the password validation. Calculating the offset to jump to is done by getting the RVA (Relative Virtual Address) of the instruction we want to jump to, and subtracting it from the RVA of the instruction we are currently at as well as the instruction length (2 bytes).
- Example:
- 0x13C1 = location of the instruction which handles correct password input.
- 0x1341 = location that we want to jump from.
- 0x13C1 - 0x1341 = 0x80
- Now we subtract the instruction length from that result (0x80 - 0x2 = 0x7E)


## Disclaimer

The "EasyCrackMe" program mentioned above, like all the other programs on [crackmes.one](https://crackmes.one), was created for the sole purpose of practicing reverse engineering skills. Please note that any attempt to use anything in this repository for any illegal or malicious activities is prohibited. Everything in this repository is intended for educational purposes.