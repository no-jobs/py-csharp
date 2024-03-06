from cffi import FFI
ffi = FFI()
ffi.cdef("const char *greeting(const char *);")
clib = ffi.dlopen("Main.dll")
msg = ffi.string(clib.greeting("山田太郎".encode())).decode()
print("msg={}".format(msg))
