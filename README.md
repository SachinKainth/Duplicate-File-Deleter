# Duplicate File Deleter

This program takes in two folder paths as input and deletes all the duplicate files.

## What gets deleted?

All the duplicate files:

1. within the first folder (and all subfolders recursively).
2. within the second folder (and all subfolders recursively).
3. between the first and second folders

## Which duplicates get deleted?

If n copies of a file are found in 1. and 2., then 1 will be kept and n-1 will be deleted.  There is no specific logic that picks which copy to keep and which copies to delete.

if a duplicate is found in 3. then the duplicate from the second folder is deleted.

## What is a duplicate?

Having the exact same file name does not make a file a duplicate.  A file is only considered a duplicate if it contains the exact same bytes.

## Usage

DFD.exe \<folder 1\> \<folder 2\>

## Warning

Please do not pass in the same folder path (nor a path for one folder that is a subfolder of the other) for the first and second folders.
