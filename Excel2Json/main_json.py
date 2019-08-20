# -*- coding: utf-8 -*-
import sys
import os
from json_generate import *

reload(sys)
sys.setdefaultencoding('utf8')

if __name__ == '__main__':

    if len(sys.argv) == 1:
        print 'ERROR: please specify the dir of excel as the paramter'
    else:
        config_manager_fields = ''
        config_manager_properties = ''
        config_manager_loads = ''
        config_items = ''

        json_generate = JSONGenerate()

        for parent, dirnames, filenames in os.walk(sys.argv[1]):
            for filename in filenames:

                if os.path.splitext(filename)[1] != '.xls' and os.path.splitext(filename)[1] != '.xlsx':
                    continue

                # LINUX系统下判断是不是临时文件
                if filename[:2] == '~$':
                    continue

                print '===================================================='
                print filename

                full_name = os.path.join(parent, filename)

                # JSON
                json_generate.load_xls_file(full_name)
                json_generate.parse_excel()
                json_generate.export_json()

        print '==========================END======================='




