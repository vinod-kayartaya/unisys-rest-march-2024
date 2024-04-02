cmake_minimum_required(VERSION 3.8)

project(CMakeRestEndpoint)

file(GLOB SOURCES "*.cpp")
add_executable(CMakeRestEndpoint ${SOURCES})

set(INCLUDE_PATHS "headers" "E:\\cpp_libs\\crow\\include" "E:\\cpp_libs\\boost" "E:\\cpp_libs\\nlohmann\\include")

target_include_directories(CMakeRestEndpoint PUBLIC ${INCLUDE_PATHS})