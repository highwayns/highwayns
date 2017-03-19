//
// Created by benben on 7/29/15.
//

#ifndef BASE_IMLOG_H
#define BASE_IMLOG_H

#include <stdarg.h>
#include <stdio.h>


class ImLog
{
public:
    static void Logi(const char *format, ...) {
        va_list args;
        va_start(args, format);
        char szBuffer[4096];
        vsnprintf(szBuffer, sizeof(szBuffer), format, args);
        va_end(args);
        printf("%s\n", szBuffer);
    }

    static void Loge(const char *format, ...) {
        va_list args;
        va_start(args, format);
        char szBuffer[4096];
        vsnprintf(szBuffer, sizeof(szBuffer), format, args);
        va_end(args);
        printf("%s\n", szBuffer);
    }

    static void Logd(const char *format, ...) {
        va_list args;
        va_start(args, format);
        char szBuffer[4096];
        vsnprintf(szBuffer, sizeof(szBuffer), format, args);
        va_end(args);
        printf("%s\n", szBuffer);
    }

    static void Logw(const char *format, ...) {
        va_list args;
        va_start(args, format);
        char szBuffer[4096];
        vsnprintf(szBuffer, sizeof(szBuffer), format, args);
        va_end(args);
        printf("%s\n", szBuffer);
    }

    static void Logt(const char *format, ...) {
        va_list args;
        va_start(args, format);
        char szBuffer[4096];
        vsnprintf(szBuffer, sizeof(szBuffer), format, args);
        va_end(args);
        printf("%s\n", szBuffer);
    }
};

#include "log4cxx/logger.h"
#include "log4cxx/basicconfigurator.h"
#include "log4cxx/propertyconfigurator.h"
#include "log4cxx/helpers/exception.h"

using namespace log4cxx;

#define MAX_LOG_LENGTH   1024 * 10
#define WATCH_DELAY_TIME     10 * 1000

class EgLog 
{
    LoggerPtr _logger;
public:
    EgLog(const char* module_name) {
        PropertyConfigurator::configureAndWatch("log4cxx.properties", WATCH_DELAY_TIME);
        _logger = Logger::getLogger(module_name);
    }
    
    virtual ~EgLog() {}
    
    void Trace(const char* loginfo) { _logger->trace(loginfo); }
    void Debug(const char* loginfo) { _logger->debug(loginfo); }
    void Info(const char* loginfo) { _logger->info(loginfo); }
    void Warn(const char* loginfo) { _logger->warn(loginfo); }
    void Error(const char* loginfo) { _logger->error(loginfo); }
    void Fatal(const char* loginfo) { _logger->fatal(loginfo); }
    
    static void Logi(const char *format, ...) {
        static EgLog s_info("INFO");
        va_list args;
        va_start(args, format);
        char szBuffer[MAX_LOG_LENGTH];
        vsnprintf(szBuffer, sizeof(szBuffer), format, args);
        va_end(args);
        s_info.Info(szBuffer);
    }
    
    static void Loge(const char *format, ...) {
        static EgLog s_error("ERROR");
        va_list args;
        va_start(args, format);
        char szBuffer[MAX_LOG_LENGTH];
        vsnprintf(szBuffer, sizeof(szBuffer), format, args);
        va_end(args);
        s_error.Error(szBuffer);
    }
    
    static void Logd(const char *format, ...) {
        static EgLog s_debug("DEBUG");
        va_list args;
        va_start(args, format);
        char szBuffer[MAX_LOG_LENGTH];
        vsnprintf(szBuffer, sizeof(szBuffer), format, args);
        va_end(args);
        s_debug.Debug(szBuffer);
    }    

    static void Logw(const char *format, ...) {
        static EgLog s_warn("WARN");
        va_list args;
        va_start(args, format);
        char szBuffer[MAX_LOG_LENGTH];
        vsnprintf(szBuffer, sizeof(szBuffer), format, args);
        va_end(args);
        s_warn.Warn(szBuffer);
    } 

    static void Logt(const char *format, ...) {
        static EgLog s_trace("TRACE");
        va_list args;
        va_start(args, format);
        char szBuffer[MAX_LOG_LENGTH];
        vsnprintf(szBuffer, sizeof(szBuffer), format, args);
        va_end(args);
        s_trace.Trace(szBuffer);
    }     
};


#define __FILENAME__ (strrchr(__FILE__, '/') ? (strrchr(__FILE__, '/') + 1):__FILE__)

#ifndef NO_IM_INFO_LOG
#define log(fmt, args...) EgLog::Logi("\n%s:%d\n<%s>----" fmt "\n", __FILE__, __LINE__, __FUNCTION__, ##args)
#define logi(fmt, args...) EgLog::Logi("\n%s:%d\n<%s>----" fmt "\n", __FILE__, __LINE__, __FUNCTION__, ##args)
#else
#define log(fmt, args...)
#define logi(fmt, args...)
#endif

#ifndef NO_IM_DEBUG_LOG
#define logd(fmt, args...) EgLog::Logd("\n%s:%d\n<%s>----" fmt "\n", __FILE__, __LINE__, __FUNCTION__, ##args)
#else
#define logd(fmt, args...)
#endif

#ifndef NO_IM_WARN_LOG
#define logw(fmt, args...) EgLog::Logw("\n%s:%d\n<%s>----" fmt "\n", __FILE__, __LINE__, __FUNCTION__, ##args)
#else
#define logw(fmt, args...)
#endif

#ifndef NO_IM_ERROR_LOG
#define loge(fmt, args...) EgLog::Loge("\n%s:%d\n<%s>----" fmt "\n", __FILE__, __LINE__, __FUNCTION__, ##args)
#else
#define loge(fmt, args...)
#endif

#ifndef NO_IM_TRACE_LOG
#define logt(fmt, args...) EgLog::Logt("\n%s:%d\n<%s>----" fmt "\n", __FILE__, __LINE__, __FUNCTION__, ##args)
#else
#define logt(fmt, args...)
#endif

#ifndef NO_RUNTIME_TRACE
#define RUNTIME_TRACE EgLog::Logt("********* %s:%d |<%s>*********\n", __FILE__, __LINE__, __FUNCTION__)
#else
#define RUNTIME_TRACE
#endif


#endif //BASE_IMLOG_H
