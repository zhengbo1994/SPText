<?php

class __Mustache_a4c8542f707532b1226fe76d4bd72659 extends Mustache_Template
{
    private $lambdaHelper;

    public function renderInternal(Mustache_Context $context, $indent = '')
    {
        $this->lambdaHelper = new Mustache_LambdaHelper($this->mustache, $context);
        $buffer = '';

        $buffer .= $indent . '<!DOCTYPE html>';
        $buffer .= "\n";
        $buffer .= $indent . '<html lang="en">';
        $buffer .= "\n";
        $buffer .= $indent . '	<head>';
        $buffer .= "\n";
        $buffer .= $indent . '		<meta charset="utf-8" />';
        $buffer .= "\n";
        $buffer .= $indent . '		<title>';
        $value = $this->resolveValue($context->findDot('page.title'), $context, $indent);
        $buffer .= htmlspecialchars($value, ENT_COMPAT, 'UTF-8');
        $buffer .= ' - ';
        $value = $this->resolveValue($context->findDot('site.title'), $context, $indent);
        $buffer .= htmlspecialchars($value, ENT_COMPAT, 'UTF-8');
        $buffer .= '</title>';
        $buffer .= "\n";
        $buffer .= $indent . '		<meta name="description" content="';
        $value = $this->resolveValue($context->findDot('page.description'), $context, $indent);
        $buffer .= htmlspecialchars($value, ENT_COMPAT, 'UTF-8');
        $buffer .= '" />';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '		<meta name="viewport" content="width=device-width, initial-scale=1.0" />';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '		<!-- basic styles -->';
        $buffer .= "\n";
        $buffer .= $indent . '		<link href="';
        $value = $this->resolveValue($context->findDot('path.assets'), $context, $indent);
        $buffer .= $value;
        $buffer .= '/css/bootstrap.min.css" rel="stylesheet" />';
        $buffer .= "\n";
        $buffer .= $indent . '		<link href="';
        $value = $this->resolveValue($context->findDot('path.assets'), $context, $indent);
        $buffer .= $value;
        $buffer .= '/css/bootstrap-responsive.min.css" rel="stylesheet" />';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '		<link rel="stylesheet" href="';
        $value = $this->resolveValue($context->findDot('path.assets'), $context, $indent);
        $buffer .= $value;
        $buffer .= '/css/font-awesome.min.css" />';
        $buffer .= "\n";
        $buffer .= $indent . '		<!--[if IE 7]>';
        $buffer .= "\n";
        $buffer .= $indent . '		  <link rel="stylesheet" href="';
        $value = $this->resolveValue($context->findDot('path.assets'), $context, $indent);
        $buffer .= $value;
        $buffer .= '/css/font-awesome-ie7.min.css" />';
        $buffer .= "\n";
        $buffer .= $indent . '		<![endif]-->';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '		<!-- page specific plugin styles -->';
        $buffer .= "\n";
        // 'page.styles' section
        $buffer .= $this->section42979b617a98e41dff488e76fcd4d6aa($context, $indent, $context->findDot('page.styles'));
        $buffer .= $indent . '		';
        $buffer .= "\n";
        $buffer .= $indent . '		<!-- fonts -->';
        $buffer .= "\n";
        $buffer .= $indent . '		<link rel="stylesheet" href="';
        $value = $this->resolveValue($context->findDot('site.protocol'), $context, $indent);
        $buffer .= $value;
        $buffer .= '//fonts.googleapis.com/css?family=Open+Sans:400,300" />';
        $buffer .= "\n";
        $buffer .= $indent . '		';
        $buffer .= "\n";
        $buffer .= $indent . '		<!-- ace styles -->';
        $buffer .= "\n";
        $buffer .= $indent . '		<link rel="stylesheet" href="';
        $value = $this->resolveValue($context->findDot('path.assets'), $context, $indent);
        $buffer .= $value;
        $buffer .= '/css/ace.min.css" />';
        $buffer .= "\n";
        $buffer .= $indent . '		<link rel="stylesheet" href="';
        $value = $this->resolveValue($context->findDot('path.assets'), $context, $indent);
        $buffer .= $value;
        $buffer .= '/css/ace-responsive.min.css" />';
        $buffer .= "\n";
        $buffer .= $indent . '		<link rel="stylesheet" href="';
        $value = $this->resolveValue($context->findDot('path.assets'), $context, $indent);
        $buffer .= $value;
        $buffer .= '/css/ace-skins.min.css" />';
        $buffer .= "\n";
        $buffer .= $indent . '		<!--[if lte IE 8]>';
        $buffer .= "\n";
        $buffer .= $indent . '		  <link rel="stylesheet" href="';
        $value = $this->resolveValue($context->findDot('path.assets'), $context, $indent);
        $buffer .= $value;
        $buffer .= '/css/ace-ie.min.css" />';
        $buffer .= "\n";
        $buffer .= $indent . '		<![endif]-->';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '		<!-- inline styles if any -->';
        $buffer .= "\n";
        // 'page.inline_styles' section
        $buffer .= $this->section1dda7802f66627e558183a339bad9568($context, $indent, $context->findDot('page.inline_styles'));
        $buffer .= $indent . '	</head>';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '	<body>';
        $buffer .= "\n";
        if ($partial = $this->mustache->loadPartial('layout.topbar')) {
            $buffer .= $partial->renderInternal($context, '	');
        }
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '		<div class="container-fluid" id="main-container">';
        $buffer .= "\n";
        if ($partial = $this->mustache->loadPartial('layout.sidenav')) {
            $buffer .= $partial->renderInternal($context, '		');
        }
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '			<div id="main-content" class="clearfix">';
        $buffer .= "\n";
        if ($partial = $this->mustache->loadPartial('layout.breadcrumbs')) {
            $buffer .= $partial->renderInternal($context, '					');
        }
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '					<div id="page-content" class="clearfix">';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        // 'page.no-header' inverted section
        $value = $context->findDot('page.no-header');
        if (empty($value)) {
            
            $buffer .= $indent . '						<div class="page-header position-relative">';
            $buffer .= "\n";
            $buffer .= $indent . '							<h1>';
            $value = $this->resolveValue($context->findDot('page.title'), $context, $indent);
            $buffer .= htmlspecialchars($value, ENT_COMPAT, 'UTF-8');
            $buffer .= ' ';
            // 'page.description' section
            $buffer .= $this->sectionEe5e79d5d4ce8324f5712e25474a1426($context, $indent, $context->findDot('page.description'));
            $buffer .= '</h1>';
            $buffer .= "\n";
            $buffer .= $indent . '						</div><!--/.page-header-->';
            $buffer .= "\n";
        }
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '						<div class="row-fluid">';
        $buffer .= "\n";
        $buffer .= $indent . '						<!-- PAGE CONTENT BEGINS HERE -->';
        $buffer .= "\n";
        if ($partial = $this->mustache->loadPartial('page.content')) {
            $buffer .= $partial->renderInternal($context, '');
        }
        $buffer .= $indent . '						<!-- PAGE CONTENT ENDS HERE -->';
        $buffer .= "\n";
        $buffer .= $indent . '						 </div><!--/row-->';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '					</div><!--/#page-content-->';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        if ($partial = $this->mustache->loadPartial('layout.settings')) {
            $buffer .= $partial->renderInternal($context, '					');
        }
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '			</div><!--/#main-content -->';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '		</div><!--/.fluid-container#main-container-->';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '		<a href="#" id="btn-scroll-up" class="btn btn-small btn-inverse">';
        $buffer .= "\n";
        $buffer .= $indent . '			<i class="icon-double-angle-up icon-only bigger-110"></i>';
        $buffer .= "\n";
        $buffer .= $indent . '		</a>';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '		<!-- basic scripts -->';
        $buffer .= "\n";
        $buffer .= $indent . '		<script src="';
        $value = $this->resolveValue($context->findDot('site.protocol'), $context, $indent);
        $buffer .= $value;
        $buffer .= '//ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>';
        $buffer .= "\n";
        $buffer .= $indent . '		<script type="text/javascript">';
        $buffer .= "\n";
        $buffer .= $indent . '		window.jQuery || document.write("<script src=\'';
        $value = $this->resolveValue($context->findDot('path.assets'), $context, $indent);
        $buffer .= $value;
        $buffer .= '/js/jquery-1.9.1.min.js\'>"+"<"+"/script>");';
        $buffer .= "\n";
        $buffer .= $indent . '		</script>';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '		<script src="';
        $value = $this->resolveValue($context->findDot('path.assets'), $context, $indent);
        $buffer .= $value;
        $buffer .= '/js/bootstrap.min.js"></script>';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '		<!-- page specific plugin scripts -->';
        $buffer .= "\n";
        // 'page.ie_scripts' section
        $buffer .= $this->sectionCa0cd0cccfcdc47b4e3927ed8a499fd4($context, $indent, $context->findDot('page.ie_scripts'));
        $buffer .= $indent . '';
        $buffer .= "\n";
        // 'page.scripts' section
        $buffer .= $this->section898db1716e9455242367f32ed6a95b6c($context, $indent, $context->findDot('page.scripts'));
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '		<!-- ace scripts -->';
        $buffer .= "\n";
        $buffer .= $indent . '		<script src="';
        $value = $this->resolveValue($context->findDot('path.assets'), $context, $indent);
        $buffer .= $value;
        $buffer .= '/js/ace-elements.min.js"></script>';
        $buffer .= "\n";
        $buffer .= $indent . '		<script src="';
        $value = $this->resolveValue($context->findDot('path.assets'), $context, $indent);
        $buffer .= $value;
        $buffer .= '/js/ace.min.js"></script>';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '		<!-- inline scripts related to this page -->';
        $buffer .= "\n";
        // 'page.inline_scripts' section
        $buffer .= $this->section69dd050c52545f313f6fd6d9c4b5ceaa($context, $indent, $context->findDot('page.inline_scripts'));
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '	</body>';
        $buffer .= "\n";
        $buffer .= $indent . '</html>';
        $buffer .= "\n";

        return $buffer;
    }

    private function section42979b617a98e41dff488e76fcd4d6aa(Mustache_Context $context, $indent, $value)
    {
        $buffer = '';
        if (!is_string($value) && is_callable($value)) {
            $source = '
		<link rel="stylesheet" href="{{{path.assets}}}/css/{{{.}}}" />
		';
            $buffer .= $this->mustache
                ->loadLambda((string) call_user_func($value, $source, $this->lambdaHelper))
                ->renderInternal($context, $indent);
        } elseif (!empty($value)) {
            $values = $this->isIterable($value) ? $value : array($value);
            foreach ($values as $value) {
                $context->push($value);
                $buffer .= $indent . '		<link rel="stylesheet" href="';
                $value = $this->resolveValue($context->findDot('path.assets'), $context, $indent);
                $buffer .= $value;
                $buffer .= '/css/';
                $value = $this->resolveValue($context->last(), $context, $indent);
                $buffer .= $value;
                $buffer .= '" />';
                $buffer .= "\n";
                $context->pop();
            }
        }
    
        return $buffer;
    }

    private function section1dda7802f66627e558183a339bad9568(Mustache_Context $context, $indent, $value)
    {
        $buffer = '';
        if (!is_string($value) && is_callable($value)) {
            $source = '
		<style type="text/css">
		{{{page.inline_styles}}}
		</style>
		';
            $buffer .= $this->mustache
                ->loadLambda((string) call_user_func($value, $source, $this->lambdaHelper))
                ->renderInternal($context, $indent);
        } elseif (!empty($value)) {
            $values = $this->isIterable($value) ? $value : array($value);
            foreach ($values as $value) {
                $context->push($value);
                $buffer .= $indent . '		<style type="text/css">';
                $buffer .= "\n";
                $buffer .= $indent . '		';
                $value = $this->resolveValue($context->findDot('page.inline_styles'), $context, $indent);
                $buffer .= $value;
                $buffer .= '';
                $buffer .= "\n";
                $buffer .= $indent . '		</style>';
                $buffer .= "\n";
                $context->pop();
            }
        }
    
        return $buffer;
    }

    private function sectionEe5e79d5d4ce8324f5712e25474a1426(Mustache_Context $context, $indent, $value)
    {
        $buffer = '';
        if (!is_string($value) && is_callable($value)) {
            $source = '<small><i class="icon-double-angle-right"></i> {{page.description}}</small>';
            $buffer .= $this->mustache
                ->loadLambda((string) call_user_func($value, $source, $this->lambdaHelper))
                ->renderInternal($context, $indent);
        } elseif (!empty($value)) {
            $values = $this->isIterable($value) ? $value : array($value);
            foreach ($values as $value) {
                $context->push($value);
                $buffer .= '<small><i class="icon-double-angle-right"></i> ';
                $value = $this->resolveValue($context->findDot('page.description'), $context, $indent);
                $buffer .= htmlspecialchars($value, ENT_COMPAT, 'UTF-8');
                $buffer .= '</small>';
                $context->pop();
            }
        }
    
        return $buffer;
    }

    private function sectionCa0cd0cccfcdc47b4e3927ed8a499fd4(Mustache_Context $context, $indent, $value)
    {
        $buffer = '';
        if (!is_string($value) && is_callable($value)) {
            $source = '
		<!--[if lte IE {{version}}]>
		  <script src="{{{path.assets}}}/js/{{name}}"></script>
		<![endif]-->
		';
            $buffer .= $this->mustache
                ->loadLambda((string) call_user_func($value, $source, $this->lambdaHelper))
                ->renderInternal($context, $indent);
        } elseif (!empty($value)) {
            $values = $this->isIterable($value) ? $value : array($value);
            foreach ($values as $value) {
                $context->push($value);
                $buffer .= $indent . '		<!--[if lte IE ';
                $value = $this->resolveValue($context->find('version'), $context, $indent);
                $buffer .= htmlspecialchars($value, ENT_COMPAT, 'UTF-8');
                $buffer .= ']>';
                $buffer .= "\n";
                $buffer .= $indent . '		  <script src="';
                $value = $this->resolveValue($context->findDot('path.assets'), $context, $indent);
                $buffer .= $value;
                $buffer .= '/js/';
                $value = $this->resolveValue($context->find('name'), $context, $indent);
                $buffer .= htmlspecialchars($value, ENT_COMPAT, 'UTF-8');
                $buffer .= '"></script>';
                $buffer .= "\n";
                $buffer .= $indent . '		<![endif]-->';
                $buffer .= "\n";
                $context->pop();
            }
        }
    
        return $buffer;
    }

    private function section898db1716e9455242367f32ed6a95b6c(Mustache_Context $context, $indent, $value)
    {
        $buffer = '';
        if (!is_string($value) && is_callable($value)) {
            $source = '
		<script src="{{{path.assets}}}/js/{{{.}}}"></script>
		';
            $buffer .= $this->mustache
                ->loadLambda((string) call_user_func($value, $source, $this->lambdaHelper))
                ->renderInternal($context, $indent);
        } elseif (!empty($value)) {
            $values = $this->isIterable($value) ? $value : array($value);
            foreach ($values as $value) {
                $context->push($value);
                $buffer .= $indent . '		<script src="';
                $value = $this->resolveValue($context->findDot('path.assets'), $context, $indent);
                $buffer .= $value;
                $buffer .= '/js/';
                $value = $this->resolveValue($context->last(), $context, $indent);
                $buffer .= $value;
                $buffer .= '"></script>';
                $buffer .= "\n";
                $context->pop();
            }
        }
    
        return $buffer;
    }

    private function section69dd050c52545f313f6fd6d9c4b5ceaa(Mustache_Context $context, $indent, $value)
    {
        $buffer = '';
        if (!is_string($value) && is_callable($value)) {
            $source = '
		<script type="text/javascript">
		{{{page.inline_scripts}}}
		</script>
		';
            $buffer .= $this->mustache
                ->loadLambda((string) call_user_func($value, $source, $this->lambdaHelper))
                ->renderInternal($context, $indent);
        } elseif (!empty($value)) {
            $values = $this->isIterable($value) ? $value : array($value);
            foreach ($values as $value) {
                $context->push($value);
                $buffer .= $indent . '		<script type="text/javascript">';
                $buffer .= "\n";
                $buffer .= $indent . '		';
                $value = $this->resolveValue($context->findDot('page.inline_scripts'), $context, $indent);
                $buffer .= $value;
                $buffer .= '';
                $buffer .= "\n";
                $buffer .= $indent . '		</script>';
                $buffer .= "\n";
                $context->pop();
            }
        }
    
        return $buffer;
    }
}
