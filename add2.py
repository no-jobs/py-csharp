from cffi import FFI
ffi = FFI()
ffi.cdef("int add2(int, int);")
clib = ffi.dlopen("Main.dll")
answer = clib.add2(11, 22)
print("answer={}".format(answer))
