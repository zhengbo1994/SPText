<?php

class __Mustache_e771361fc4b94c115b7be6dab44dc779 extends Mustache_Template
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
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '		<!-- fonts -->';
        $buffer .= "\n";
        $buffer .= $indent . '		<link rel="stylesheet" href="';
        $value = $this->resolveValue($context->findDot('site.protocol'), $context, $indent);
        $buffer .= $value;
        $buffer .= '//fonts.googleapis.com/css?family=Open+Sans:400,300" />';
        $buffer .= "\n";
        $buffer .= $indent . '';
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
        $buffer .= $indent . '		<!--[if lt IE 9]>';
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
        $buffer .= $indent . '	</head>';
        $buffer .= "\n";
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '	<body class="login-layout">';
        $buffer .= "\n";
        $buffer .= $indent . '	';
        $buffer .= "\n";
        $buffer .= $indent . '		<div class="container-fluid" id="main-container">';
        $buffer .= "\n";
        $buffer .= $indent . '			<div id="main-content">';
        $buffer .= "\n";
        $buffer .= $indent . '				<div class="row-fluid">';
        $buffer .= "\n";
        $buffer .= $indent . '					<div class="span12">';
        $buffer .= "\n";
        if ($partial = $this->mustache->loadPartial('page.content')) {
            $buffer .= $partial->renderInternal($context, '						');
        }
        $buffer .= $indent . '					</div><!--/span-->';
        $buffer .= "\n";
        $buffer .= $indent . '				</div><!--/row-->';
        $buffer .= "\n";
        $buffer .= $indent . '			</div>';
        $buffer .= "\n";
        $buffer .= $indent . '		</div><!--/.fluid-container-->';
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
        $buffer .= $indent . '';
        $buffer .= "\n";
        $buffer .= $indent . '		<!-- page specific plugin scripts -->';
        $buffer .= "\n";
        // 'page.scripts' section
        $buffer .= $this->section898db1716e9455242367f32ed6a95b6c($context, $indent, $context->findDot('page.scripts'));
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
