################################################################################
# Automatically-generated file. Do not edit!
################################################################################

# Add inputs and outputs from these tool invocations to the build variables 
CPP_SRCS += \
../SocketSession.cpp \
../main.cpp 

OBJS += \
./SocketSession.o \
./main.o 

CPP_DEPS += \
./SocketSession.d \
./main.d 


# Each subdirectory must supply rules for building sources it contributes
%.o: ../%.cpp
	@echo 'Building file: $<'
	@echo 'Invoking: GNU C++ 编译器'
	g++ -I"/home/dengxiaobo/workspace/FxLib/Database" -I"/home/dengxiaobo/workspace/FxLib/Net" -I"/home/dengxiaobo/workspace/FxLib/LuaLib" -I../../tolua -I../../mysql/include -I../../meta_header -I../../lua -O3 -Wall -c -fmessage-length=0 -MMD -MP -MF"$(@:%.o=%.d)" -MT"$(@)" -o "$@" "$<"
	@echo 'Finished building: $<'
	@echo ' '


