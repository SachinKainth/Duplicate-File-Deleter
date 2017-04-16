# Duplicate File Deleter

This program takes in two folder paths as input and deletes all the duplicate files.

## If you only need the executable file 

The DFD.exe file can be found in /Executable folder.  This is all you need to run the program (at least on Windows).

## What gets deleted?

All the duplicate files:

1. within the first folder (and all subfolders recursively).
2. within the second folder (and all subfolders recursively).
3. between the first and second folders

After the duplicates have been deleted any empty folders that remain are also deleted with the first and second folder.

## Which duplicates get deleted?

If n copies of a file are found in 1. and 2., then 1 will be kept and n-1 will be deleted.  There is no specific logic that picks which copy to keep and which copies to delete.

if a duplicate is found in 3. then the duplicate from the second folder is deleted.

## What is a duplicate?

Having the exact same file name does not make a file a duplicate.  A file is only considered a duplicate if it contains the exact same bytes.

## Usage

DFD.exe \<folder 1\> \<folder 2\>